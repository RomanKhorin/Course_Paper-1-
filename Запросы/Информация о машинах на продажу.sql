select Firm, Model, Colour, Engine, Country, Name as [Center Town], Adress from Sales_Car sc
inner join Center c on sc.Center_Id = c.Center_Id
inner join Town t on t.Town_Id = c.Town_Id;