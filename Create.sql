DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS Discs;
DROP TABLE IF EXISTS Customers;
DROP TABLE IF EXISTS Brands;

CREATE TABLE Brands (
    brand_id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    city TEXT NOT NULL,
    country TEXT NOT NULL,
    established_year INTEGER NOT NULL
);

CREATE TABLE Discs (
    disc_id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    brand_id INTEGER NOT NULL,
    stock INTEGER NOT NULL,
    price REAL NOT NULL,
    type TEXT NOT NULL,
    speed INTEGER NOT NULL,
    glide INTEGER NOT NULL,
    turn INTEGER NOT NULL,
    fade INTEGER NOT NULL,
    FOREIGN KEY (brand_id) REFERENCES Brands(brand_id) ON DELETE CASCADE
);

CREATE TABLE Customers (
    customer_id INTEGER PRIMARY KEY AUTOINCREMENT,
    first_name TEXT NOT NULL,
    last_name TEXT NOT NULL,
    email TEXT UNIQUE NOT NULL,
    phone TEXT NOT NULL
);

CREATE TABLE Orders (
    order_id INTEGER PRIMARY KEY AUTOINCREMENT,
    customer_id INTEGER NOT NULL,
    disc_id INTEGER NOT NULL,
    order_date TEXT NOT NULL,  -- SQLite uses TEXT for dates
    quantity INTEGER NOT NULL,
    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id) ON DELETE CASCADE,
    FOREIGN KEY (disc_id) REFERENCES Discs(disc_id) ON DELETE CASCADE
);
