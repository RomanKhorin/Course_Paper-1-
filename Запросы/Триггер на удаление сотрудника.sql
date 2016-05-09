create trigger delete_all_with_employee
on [dbo].[Employee]
instead of delete
as
begin
	DECLARE @X INT
	SELECT @X = Employee_Id FROM DELETED
BEGIN
	delete from Rental_Contract where Employee_Id = @X
end
begin
	delete from Sales_Contract where Employee_Id = @X
end
begin
	delete from Employee where Employee_Id = @X
end
end