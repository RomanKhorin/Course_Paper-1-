select Name as Town, Adress, c.Town_Id as [Town Code], Telephone from Center c
inner join Town t on c.Town_Id = t.Town_Id;