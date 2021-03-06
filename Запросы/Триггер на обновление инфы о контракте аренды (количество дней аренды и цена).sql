USE [Car_Center]
GO
/****** Object:  Trigger [dbo].[update_rental_contract_number_of_days]    Script Date: 30-Apr-16 17:03:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER trigger [dbo].[update_rental_contract_number_of_days]
on [dbo].[Rental_Contract] after insert
as
declare @X date, @Y date, @Z varchar(6)
select @X = I.Date_Of_Begin, @Y = I.Date_Of_End, @Z = I.Car_Number from Rental_Contract rc,
inserted I where rc.Car_Number = I.Car_Number
begin
update [dbo].[Rental_Contract]
set Number_of_days = (DATEDIFF(DAY, @X, @Y))
where Car_Number = @Z
begin
update [dbo].[Rental_Contract]
set Total_Price = Number_of_days * Price_Per_Day
where Car_Number = @Z
end
end;
