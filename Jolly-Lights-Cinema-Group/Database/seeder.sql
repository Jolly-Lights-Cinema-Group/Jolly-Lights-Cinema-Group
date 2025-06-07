INSERT INTO Location (Name, Address) VALUES
  ('Jolly Lights Rotterdam', 'Wijnhaven 133'),
  ('Jolly Lights Utrecht', 'Oudegracht 20'),
  ('Jolly Lights Amsterdam', 'P.C. Hooftstraat 15');

INSERT INTO Employee (FirstName, LastName, DateOfBirth, Address, Email, UserName, Password, Role) VALUES
  ('Employee', 'Test', '01-01-1111', 'Testway 32', 'test.employee@email.com', 'test_employee', '$2a$11$.SMz7Jv065RphmCMqtPlCu7mAUGlv.yoXcdM9zuA6ZIZhRRZ.Hg3y', 0),
  ('Admin', 'Test', '01-01-1111', 'Testway 32', 'test.admin@email.com', 'test_admin', '$2a$11$6i9nej7W5bxa8yPkNuNV5uqU00HPy7e3C9yMXZuIkTzxnl4FmJOHu', 1),
  ('Manager', 'Test', '01-01-1111', 'Testway 32', 'test.manager@email.com', 'test_manager', '$2a$11$Sduv8/gfrvfhnYP/D7twruGkMQHV/9I0oDA675YHboifvROzFcZ1m', 2);

INSERT INTO Seats (LocationId, Type, Price) VALUES 
    (1, 0, 10), 
    (1, 1, 15), 
    (1, 2, 20);

INSERT INTO Movie (Title, Duration, MinimunAge, ReleaseDate, MovieCast) VALUES 
    ('Cinderella', 45, 4, '2025-06-01', 'Cinderella, Stepsister'),
    ('Barbie', 120, 9, '2025-06-01', 'Margot Robbie, Ryan Gosling, Kate McKinnon'),
    ('Pirates of the Caribbean', 140, 12, '2025-06-01', 'Johnny Depp, Keira Knightley');

INSERT INTO ShopItem (Name, Price, Stock, LocationId, VatPercentage, MinimumAge) VALUES
  ('Popcorn', 5.00, 100, 1, 9, 0),
  ('Cola', 3.00, 100, 1, 9, 0),
  ('Beer', 6.00, 50, 1, 21, 18),
  ('Popcorn', 5.00, 100, 2, 9, 0),
  ('Cola', 3.00, 100, 2, 9, 0),
  ('Beer', 6.00, 50, 2, 21, 18),
  ('Popcorn', 5.00, 100, 3, 9, 0),
  ('Cola', 3.00, 100, 3, 9, 0),
  ('Beer', 6.00, 50, 3, 21, 18);

INSERT INTO CustomerOrder (GrandPrice, PayDate, Tax) VALUES
  (25.00, '2025-01-07', 2.50),
  (30.00, '2025-02-08', 3.00),
  (100.00, '2025-03-07', 2.50),
  (45.00, '2025-04-08', 3.00),
  (60.00, '2025-05-07', 2.50),
  (70.00, '2025-06-08', 3.00),
  (90.00, '2025-07-07', 2.50),
  (20.00, '2025-08-08', 3.00);
