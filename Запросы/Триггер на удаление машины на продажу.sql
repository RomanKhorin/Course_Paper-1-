create trigger delete_all_with_sales_car
on [dbo].[Sales_Car]
instead of delete
as
begin
	DECLARE @X INT
	SELECT @X = Car_Id FROM DELETED
begin
	delete from Sales_Contract where Car_Id = @X
end
begin
	delete from Sales_Car where Car_Id = @X
end
end;