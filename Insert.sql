-- Insert into Brands
INSERT INTO Brands (name, city, country, established_year) 
VALUES ('Innova','Rancho Cucamonga', 'USA', '1983'),
('Discraft', 'Wixom', 'USA', '1979'),
('Dynamic Discs', 'Emporia', 'USA', '2005'),
('Prodigy', 'Dalton', 'USA', '2013');
Select * From Brands;
	
-- Insert into Customers
INSERT INTO Customers (first_name, last_name, email, phone) VALUES ('John', 'Doe', 'john@example.com', '123-456-7890');
INSERT INTO Customers (first_name, last_name, email, phone) VALUES ('Jane', 'Doe', 'Jane@example.com', '908-765-321');
INSERT INTO Customers (first_name, last_name, email, phone) VALUES ('Paul', 'McBeth', 'Paul@example.com', '316-456-7890');
INSERT INTO Customers (first_name, last_name, email, phone) VALUES ('Ricky', 'Wysoki', 'Ricky@example.com', '620-456-7890');
SELECT * FROM Customers;

-- Insert into Discs
INSERT INTO Discs (name, brand_id, type, speed, glide, turn, fade, price, stock) 
VALUES ('Destroyer', 1, 'Driver', 12, 5, -1, 3, 17.99, 20);
INSERT INTO Discs (name, brand_id, type, speed, glide, turn, fade, price, stock) 
VALUES ('Aero', 1, 'Putter', 3, 6, 0, 0, 12.99, 50);
INSERT INTO Discs (name, brand_id, type, speed, glide, turn, fade, price, stock) 
VALUES ('Alien', 1, 'Midrange', 4, 2, 0, 1, 15.99, 25);
INSERT INTO Discs (name, brand_id, type, speed, glide, turn, fade, price, stock) 
VALUES ('Felon', 2, 'Driver', 9, 4, 0.5, 4, 17.99, 10);
SELECT * FROM Discs;

-- Insert into Orders
INSERT INTO Orders (customer_id, disc_id, order_date, quantity) VALUES (1, 1, '2025-03-01', 2);
INSERT INTO Orders (customer_id, disc_id, order_date, quantity) VALUES (2, 2, '2025-03-10', 3);
INSERT INTO Orders (customer_id, disc_id, order_date, quantity) VALUES (3, 3, '2025-03-30', 2);
INSERT INTO Orders (customer_id, disc_id, order_date, quantity) VALUES (4, 4, '2025-03-17', 5);
SELECT * FROM Orders;