USE [Car_Center]
GO
/****** Object:  Trigger [dbo].[delete_all_with_client]    Script Date: 07-May-16 14:12:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER trigger [dbo].[delete_all_with_client]
on [dbo].[Customer]
instead of delete
as
begin
	DECLARE @X INT
	SELECT @X = Customer_Id FROM DELETED
BEGIN
	delete from Rental_Contract where Customer_Id = @X
end
begin
	delete from Sales_Contract where Customer_Id = @X
end
begin
	delete from Customer where Customer_Id = @X
end
end