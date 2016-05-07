select Customer.First_Name as [Customer Name], Customer.Last_Name as [Customer Last Name],
Rental_Car.Firm, Rental_Car.Model,
--Employee.First_Name as [Employee Name], Employee.Last_Name as [Employee Last Name], Car_Number as [Car Number],
Date_Of_Begin as [Date Of Begin], Date_Of_End as [Date Of End], Rental_Contract.Number_of_days as [Number Of Days],
Rental_Contract.Price_Per_Day as [Price Per Day], Total_Price as [Total Price] from Rental_Contract
inner join Customer on Rental_Contract.Customer_Id = Customer.Customer_Id
inner join Rental_Car on Rental_Contract.Car_Number = Rental_Car.Car_Number;
--inner join Employee on Rental_Contract.Employee_Id = Employee.Employee_Id;