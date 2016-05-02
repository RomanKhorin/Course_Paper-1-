select Firm, Model, Car_Number as [Car number], Colour, Engine, Country, Description
from Rental_Car rc inner join Condition c
on rc.Condition_Number = c.Condition_Number;
