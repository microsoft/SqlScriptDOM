BEGIN TRY
    DECLARE @zero AS INT;
    SET @zero = 0;
    SELECT 1 / @zero;
END TRY
BEGIN CATCH
END CATCH

BEGIN TRY
    DECLARE @zero AS INT;
    SET @zero = 0;
    SELECT 1 / @zero;
END TRY
BEGIN CATCH
    EXECUTE usp_GetErrorInfo ;
END CATCH

BEGIN TRY
    BEGIN TRANSACTION;
    DELETE Production.Product
    WHERE ProductID = 980;
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    EXECUTE usp_GetErrorInfo ;
    IF (XACT_STATE()) = -1
        BEGIN
            PRINT N'The transaction is in an uncommittable state.' + 'Rolling back transaction.';
            ROLLBACK;
        END
    IF (XACT_STATE()) = 1
        BEGIN
            PRINT N'The transaction is committable.' + 'Committing transaction.';
            COMMIT TRANSACTION;
        END
END CATCH