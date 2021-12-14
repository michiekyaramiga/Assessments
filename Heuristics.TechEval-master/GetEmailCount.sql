USE [master]
GO

/****** Object:  UserDefinedFunction [dbo].[GetEmailCount]    Script Date: 12/13/2021 8:33:01 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[GetEmailCount] 
(
	-- Add the parameters for the function here
	@enteredEmail nvarchar(max)
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @cnt int;

	-- Add the T-SQL statements to compute the return value here
	SELECT @cnt = COUNT(*) FROM dbo.Member
	WHERE Email = @enteredEmail

	-- Return the result of the function
	RETURN @cnt

END
GO


