
----------Token User Authentication------------------------------------------------------
---------User Register without hash------------------------
USE imagesdb;
CREATE TABLE userData(
	user_id BIGINT IDENTITY(1,1) PRIMARY KEY,
	user_name VARCHAR(50) NOT NULL,
	user_password NVARCHAR(200) NOT NULL,
	);

INSERT INTO userData(user_name, user_password)
VALUES('Soham','Soham@123'),('Kenvi', 'Kenvi@123');

SELECT * FROM userData;
TRUNCATE TABLE userData;

--ALTER PROCEDURE InsertUserData
--(
--	@userName VARCHAR(50),
--	@userPassword NVARCHAR(200)
--)
--AS
--BEGIN
--	SET NOCOUNT ON;
--	BEGIN TRY
--		BEGIN TRANSACTION;

--		INSERT INTO userData(user_name, user_password)
--		VALUES (@userName, @userPassword);

--		COMMIT;
--	END TRY
--	BEGIN CATCH
--		IF @@ERROR <> 0
--		BEGIN
--			IF @@TRANCOUNT > 0
--				ROLLBACK;
--		END
--	END CATCH
--END;

exec InsertUserData	
	@userName ="Rushi",
	@userPassword = "Rushi@123";

-----------Login Validate without hash------------
--ALTER PROCEDURE ValidateUser
--    @Username VARCHAR(20),
--    @Password VARCHAR(20)
--AS
--BEGIN
--    SET NOCOUNT ON;

--    DECLARE @IsValidUser BIT;

--	-- Validate user
--    SELECT @IsValidUser = CASE WHEN EXISTS (
--        SELECT 1 FROM userData WHERE user_name = @Username AND User_Password = @Password
--    ) THEN 1 ELSE 0 END;

--    -- Return the result indicating if the user is valid
--    SELECT @IsValidUser AS IsValidUser;
--END;

EXEC ValidateUser 
	@Username = 'soham',
	@Password = 'kenvi';

-----------------------------------------------------------------------------------------
-----------------------Hash Password Store and validate-------------------------------

-----------Register new user with SHA Hash Password store-----------------------------
ALTER PROCEDURE InsertUserData
(
    @userName VARCHAR(50),
    @userPassword NVARCHAR(200) 
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Check if the username already exists
        IF EXISTS (SELECT 1 FROM userData WHERE user_name = @userName)
        BEGIN
            THROW 50000, 'Username already exists.', 1;
			-- 50000 is error number , custom message, 1- severity level.
        END
        ELSE
        BEGIN
            
            DECLARE @hashedPassword NVARCHAR(200);
            SET @hashedPassword = CONVERT(VARCHAR(200), HASHBYTES('SHA2_256', @userPassword), 1);
			--converted into SHA-256 and  and again convert into VARCHAR and then stored.

            INSERT INTO userData(user_name, user_password, Request_token)
            VALUES (@userName, @hashedPassword, 'Default');
        END

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@ERROR <> 0
        BEGIN
            IF @@TRANCOUNT > 0
                ROLLBACK;
            THROW;
        END
    END CATCH
END;


exec InsertUserData
 @userName = 'soham66',
 @userPassword = 'password@123';

----------------Login Authenticate user with SHA_256 Hash password check-----------------

ALTER PROCEDURE ValidateUser
    @Username VARCHAR(50),
    @Password NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IsValidUser BIT;

    -- Retrieve the stored hashed password for the given username
    DECLARE @StoredPassword NVARCHAR(200);
    SELECT @StoredPassword = user_password FROM userData WHERE user_name = @Username;

    IF @StoredPassword IS NOT NULL
    BEGIN
        -- Hash the entered password before comparing it
        DECLARE @EnteredPasswordHashed NVARCHAR(200);
        SET @EnteredPasswordHashed = CONVERT(NVARCHAR(200), HASHBYTES('SHA2_256', @Password), 1);

        -- Debug statement: Output the entered and stored passwords
        PRINT 'Entered Password: ' + @EnteredPasswordHashed;
        PRINT 'Stored Password: ' + @StoredPassword;

        -- Compare the hashed passwords
        IF @EnteredPasswordHashed = @StoredPassword
            SET @IsValidUser = 1;
        ELSE
            SET @IsValidUser = 0;
    END
    ELSE
        SET @IsValidUser = 0;

    -- Debug statement: Output the result
    PRINT 'IsValidUser: ' + CAST(@IsValidUser AS NVARCHAR(1));

    -- Return the result indicating if the user is valid
    SELECT @IsValidUser AS IsValidUser;
END;


EXEC ValidateUser 
	@Username = 'soham44',
	@Password = 'password@123';	

SELECT * FROM userData;

-- Validate the Refresh Token 

ALTER PROCEDURE RefreshTokenValidate
    @userName VARCHAR(50),
    @RequestToken NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsValidToken BIT;
	 DECLARE @ExpirationTime DATETIME;

    -- Validate user and their Refresh token
    SELECT @IsValidToken = CASE WHEN EXISTS (
        SELECT 1 FROM userData WHERE user_name = @userName AND Request_token = @RequestToken
    ) THEN 1 ELSE 0 END;

	-- Check expiration time if the token is valid
    IF @IsValidToken = 1
    BEGIN
        SELECT @ExpirationTime = ExpirationTime FROM userData WHERE user_name = @userName AND Request_token = @RequestToken;

        -- Check if token has expired
        IF @ExpirationTime < GETDATE()
        BEGIN
            SELECT 'EXPIRED' AS IsValidToken;
            RETURN;
        END
    END

    -- Return the result indicating if the user is valid
    SELECT @IsValidToken AS IsValidToken;
END;


EXEC RefreshTokenValidate
	@userName = 'soham44',
	@RequestToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzb2hhbSIsImp0aSI6ImNjMzc0N2QzLWQ1MmUtNDRlZC1iMzI2LWM3ODRiNjM1MmRlNyIsImlhdCI6IjE2ODY4MjY3MTEiLCJleHAiOjE2ODY4MjczMTEsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcyODAiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MjgwIn0.43DuTRcSVWznE5nqg9LYyDbc-mVfH5NS72ualHIlSU4';

-- add or update the request token

ALTER PROCEDURE Request_token_crud
    @userName VARCHAR(50),
    @RequestToken TEXT,
	@ExpirationTimeInMinutes INT
AS
BEGIN
    SET NOCOUNT ON;

	DECLARE @ExpirationTime DATETIME;
    SET @ExpirationTime = DATEADD(MINUTE, @ExpirationTimeInMinutes, GETDATE());

    BEGIN TRY
        BEGIN TRANSACTION;
            UPDATE userData
            SET Request_token = @RequestToken,
				ExpirationTime = @ExpirationTime
            WHERE user_name = @userName;
        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@ERROR <> 0
        BEGIN
            IF @@TRANCOUNT > 0
                ROLLBACK;
            THROW;
        END
    END CATCH;
END;

exec Request_token_crud
	@userName = "soham",
	@RequestToken ="vd",
	@ExpirationTimeInMinutes = 90;

	select * from userData;

-------- Update the refreshtoken expiration Time-----------

ALTER PROCEDURE Update_refresh_expiration_time
	@userName VARCHAR(50),
	@expireTime INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @expTime DATETIME;
	SET @expTime = DATEADD(MINUTE, @expireTime, GETDATE());
	BEGIN TRY
		BEGIN TRANSACTION
			
			UPDATE userData 
				SET ExpirationTime = @expTime
			WHERE user_name = @userName 
		COMMIT;
	END TRY
	BEGIN CATCH
		IF @@ERROR <> 0
			BEGIN
				IF @@TRANCOUNT > 0
					ROLLBACK;
				THROW;
			END
	END CATCH;
END;

Exec Update_refresh_expiration_time	
	@userName = 'soham',
	@expireTime = 20;

