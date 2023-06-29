 -- get the first day of the week 

 USE CIPlatform;

 ALTER PROCEDURE firstDayOfWeek
	@date DATETIME
 AS
 BEGIN 
	DECLARE @firstDay DATETIME;

    DECLARE @gettingDay INT = DATEPART(WEEKDAY, @date) -1 ;
    SET @firstDay = DATEADD(DAY, -@gettingDay, @date);

    -- Print first day
    SELECT @firstDay AS [First Day];
 END;

EXEC firstDayOfWeek
    @date = '2023-06-07';

 -- split the string

 ALTER PROCEDURE SplitString
    @inputString VARCHAR(MAX),
    @separator VARCHAR(2)
AS
BEGIN
    DECLARE @splitTable TABLE (newString VARCHAR(MAX));

    WHILE CHARINDEX(@separator, @inputString) > 0
    BEGIN
        INSERT INTO @splitTable (newString)
        SELECT SUBSTRING(@inputString, 1, CHARINDEX(@separator, @inputString) - 1);

		--update string
        SET @inputString = SUBSTRING(@inputString, CHARINDEX(@separator, @inputString) + LEN(@separator), LEN(@inputString));
    END;
    
    INSERT INTO @splitTable (newString) VALUES (@inputString);

	--PRINT DATA
    SELECT newString FROM @splitTable;
END;
	
EXEC SplitString
    @inputString = 'Hello,TatvaSoft,Members',
    @separator = ',';


-- practice

SELECT CHARINDEX('mr', 'best programming java language');
-- N-CS
SELECT CHARINDEX('MM' , 'best programming java language');
-- by start the position
SELECT CHARINDEX('MM' , 'best programming java language', 13);
-- case sencitive
SELECT CHARINDEX('MM' , 'best programming java language' COLLATE Latin1_General_CS_AS);
	
--substring
SELECT SUBSTRING('Hi this is tatvasoft', 4,9);

--current date 
SELECT GETDATE();

