select First_Name as [First Name], Last_Name as [Last Name], Birth_Date as [Date Of Birth], Name as Town, Adress from Employee e
inner join Center c on e.Center_Id = c.Center_Id
inner join Town t on c.Town_Id = t.Town_Id;