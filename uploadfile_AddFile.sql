
-- =============================================
--	Name				Date			Description
--	thientc			05/08/2019			Thêm File
-- =============================================
ALTER PROCEDURE uploadfile_AddFile
	-- Add the parameters for the stored procedure here
    @FileID UNIQUEIDENTIFIER ,
    @NameFile nvarchar(50),		
	@UploadPerson nvarchar(50)
AS
    BEGIN				
        INSERT INTO FileUpload
                ( FileID ,
                  NameFile ,
                  
                  UploadPerson                   
                )
		OUTPUT  Inserted.FileID
        VALUES  ( @FileID ,
                  @NameFile ,
                  
                  @UploadPerson                  
                )				
    END