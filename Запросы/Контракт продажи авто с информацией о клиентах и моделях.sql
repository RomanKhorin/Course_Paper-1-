select Customer.First_Name as [Customer Name], Customer.Last_Name as [Customer Last Name],
Sales_Car.Firm, Sales_Car.Model, Sales_Car.Engine,
--Employee.First_Name as [Employee Name], Employee.Last_Name as [Employee Last Name],
Date as [Contract Date], Price as [Total Cost] from Sales_Contract sc
inner join Sales_Car on sc.Car_Id = Sales_Car.Car_Id
inner join Customer on sc.Customer_Id = Customer.Customer_Id;
--inner join Employee on sc.Employee_Id = Employee.Employee_Id;