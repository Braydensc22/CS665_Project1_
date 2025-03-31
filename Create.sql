create database if not exists DiscGolfStore;
use DiscGolfStore;
drop table if exists Discs;
CREATE TABLE Discs (
	disc_id INTEGER PRIMARY KEY auto_increment, -- unique disc id
    name TEXT NOT NULL, -- Name of the disc
    brand_id INTEGER NOT NULL, -- Brand reference
	stock INTEGER NOT NULL, -- Number of discs available
	price REAL NOT NULL, -- Price of the disc
-- everything below this are characteristics of the disc
	type TEXT NOT NULL, -- driver, midrange, putter
    speed INTEGER NOT NULL, -- how fast the disc should fly(1-14)
    glide INTEGER NOT NULL, -- how much the disc likes to stay in the air(1-7)
    turn INTEGER NOT NULL, -- at the beginning of the flight how far it goes right(-5 to 1)
    fade INTEGER NOT NULL, -- at the end of the flight how far it goes left(0-5)
-- foreign key MUST go last
    FOREIGN KEY (brand_id) REFERENCES Brands(brand_id) ON DELETE CASCADE
);
desc Discs;
/* Functional Dependencies:
   disc_id → name, brand_id, type, speed, glide, turn, fade, price, stock
   brand_id → (Dependency in Brands table) */

drop table if exists Customers;
   CREATE TABLE Customers(
	customer_id INTEGER PRIMARY KEY auto_increment, -- unique id 
	first_name TEXT NOT NULL, -- first name
	last_name TEXT NOT NULL, -- last name 
	email VARCHAR(225) UNIQUE NOT NULL, -- email MUST be unique
	phone TEXT NOT NULL -- contact number 
);
desc Customers;
/* func Dependencies 
	customer_id-> first and last name, email, phone
*/
Drop table if exists Orders;
DROP table if exists Discs;
DROP TABLE IF EXISTS Brands;
CREATE TABLE Brands (
    brand_id INTEGER PRIMARY KEY AUTO_INCREMENT, -- unique id for the brand, auto-incremented
    name VARCHAR(255) NOT NULL, -- name of brand 
    city VARCHAR(255) NOT NULL, -- city based out of 
    country VARCHAR(255) NOT NULL, -- country based out of 
    established_year INTEGER NOT NULL -- year founded 
);
desc Brands;
/* Functional Dependencies:
   brand_id → name, city, country, established_year */
drop table if exists Orders;
CREATE TABLE Orders (
	order_id INTEGER PRIMARY KEY auto_increment, -- unique order id
	customer_id INTEGER NOT NULL, -- customer reference
	disc_id INTEGER NOT NULL, -- disc reference
	order_date DATE NOT NULL, -- date of ORDER
	quantity INTEGER NOT NULL, -- number of discs purchased
	FOREIGN KEY (customer_id) REFERENCES Customers(customer_id) ON DELETE CASCADE, -- if id is deleted all references will be deleted
	FOREIGN KEY (disc_id) REFERENCES Discs(disc_id) ON DELETE CASCADE -- if disc id is deleted all references will be deleted 
);
desc Orders;
/* func Dependencies 
	order_id-> customer_id, disc_id, order_date, quantity
	customer_id-> depends on customer TABLE
	disc_id-> depends on discs table */
