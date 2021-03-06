USE [Car_Center]
GO
/****** Object:  Trigger [dbo].[delete_all_with_rental_car]    Script Date: 07-May-16 14:30:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER trigger [dbo].[delete_all_with_rental_car]
on [dbo].[Rental_Car]
instead of delete
as
begin
	DECLARE @X varchar(6)
	SELECT @X = Car_Number FROM DELETED
begin
	delete from Rental_Contract where Car_Number = @X
end
begin
	delete from Rental_Car where Car_Number = @X
end
end;