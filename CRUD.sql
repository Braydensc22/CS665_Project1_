-- CRUD
-- Create was previous file
-- Read
SELECT * FROM Discs; -- this is also done under each table in the insert to check the insertion
SELECT * FROM Customers;
SELECT * FROM Brands;
SELECT * FROM Orders;
-- Update
UPDATE Discs SET stock = stock - 2 WHERE disc_id = 1; -- subtracts 2 discs from id 1
UPDATE Discs SET stock = 45 WHERE name = 'Alien'; -- adds 45 aliens to inventory
SELECT * FROM Discs; -- shows updates 
-- Delete a customer (will cascade delete their orders)
DELETE FROM Customers WHERE customer_id = 2; -- deletes the customer and order from using cascade 
SELECT * FROM Customers; -- shows changes to customer 