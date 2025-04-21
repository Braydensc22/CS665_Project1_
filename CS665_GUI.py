import sqlite3
import tkinter as tk
from tkinter import messagebox, ttk

# Initialize database
conn = sqlite3.connect("app.db")
cursor = conn.cursor()

# Run provided SQL scripts
with open("Create.sql") as f:
    cursor.executescript(f.read())
with open("Insert.sql") as f:
    cursor.executescript(f.read())
conn.commit()

# GUI App
root = tk.Tk()
root.title("Simple DB GUI")
root.geometry("1000x750")

# UI Components
query_output = tk.Text(root, height=15, width=110)
query_output.pack(pady=10)


def run_query(sql):
    try:
        cursor.execute(sql)
        rows = cursor.fetchall()
        query_output.delete("1.0", tk.END)
        for row in rows:
            query_output.insert(tk.END, str(row) + "\n")
        conn.commit()
    except Exception as e:
        messagebox.showerror("Error", str(e))

# Example Queries
def show_all_customers():
    run_query("SELECT * FROM Customers")

def show_all_discs():
    run_query("SELECT * FROM Discs")

def show_orders_with_customer_names():
    run_query('''
        SELECT o.order_id, c.first_name || ' ' || c.last_name AS customer_name, o.order_date
        FROM Orders o
        JOIN Customers c ON o.customer_id = c.customer_id
    ''')

def show_most_popular_disc():
    run_query('''
        SELECT d.name, SUM(o.quantity) as total_sold
        FROM Orders o
        JOIN Discs d ON o.disc_id = d.disc_id
        GROUP BY d.name
        ORDER BY total_sold DESC
        LIMIT 1
    ''')

# Buttons
btn_frame = tk.Frame(root)
btn_frame.pack()

tk.Button(btn_frame, text="Show All Customers", command=show_all_customers).grid(row=0, column=0, padx=5, pady=5)
tk.Button(btn_frame, text="Show All Discs", command=show_all_discs).grid(row=0, column=1, padx=5, pady=5)
tk.Button(btn_frame, text="Orders with Customer Names", command=show_orders_with_customer_names).grid(row=0, column=2, padx=5, pady=5)
tk.Button(btn_frame, text="Most Popular Disc", command=show_most_popular_disc).grid(row=0, column=3, padx=5, pady=5)

# Custom SQL
query_entry = tk.Entry(root, width=100)
query_entry.pack(pady=5)
tk.Button(root, text="Run Custom SQL", command=lambda: run_query(query_entry.get())).pack(pady=5)

# CRUD: Customers
crud_frame = tk.LabelFrame(root, text="Customer Management")
crud_frame.pack(pady=10)

tk.Label(crud_frame, text="Customer ID (for Update/Delete):").grid(row=0, column=0)
customer_id_entry = tk.Entry(crud_frame)
customer_id_entry.grid(row=0, column=1)

tk.Label(crud_frame, text="First Name:").grid(row=1, column=0)
first_name_entry = tk.Entry(crud_frame)
first_name_entry.grid(row=1, column=1)

tk.Label(crud_frame, text="Last Name:").grid(row=2, column=0)
last_name_entry = tk.Entry(crud_frame)
last_name_entry.grid(row=2, column=1)

tk.Label(crud_frame, text="Email:").grid(row=3, column=0)
email_entry = tk.Entry(crud_frame)
email_entry.grid(row=3, column=1)

tk.Label(crud_frame, text="Phone:").grid(row=4, column=0)
phone_entry = tk.Entry(crud_frame)
phone_entry.grid(row=4, column=1)

# CRUD functions: Customers
def add_customer():
    try:
        cursor.execute("INSERT INTO Customers (first_name, last_name, email, phone) VALUES (?, ?, ?, ?)",
                       (first_name_entry.get(), last_name_entry.get(), email_entry.get(), phone_entry.get()))
        conn.commit()
        messagebox.showinfo("Success", "Customer added successfully.")
    except Exception as e:
        messagebox.showerror("Error", str(e))

def update_customer():
    try:
        cursor.execute("UPDATE Customers SET first_name=?, last_name=?, email=?, phone=? WHERE customer_id=?",
                       (first_name_entry.get(), last_name_entry.get(), email_entry.get(), phone_entry.get(), customer_id_entry.get()))
        if cursor.rowcount == 0:
            raise Exception("Customer ID not found.")
        conn.commit()
        messagebox.showinfo("Success", "Customer updated successfully.")
    except Exception as e:
        messagebox.showerror("Error", str(e))

def delete_customer():
    try:
        cursor.execute("DELETE FROM Customers WHERE customer_id=?", (customer_id_entry.get(),))
        if cursor.rowcount == 0:
            raise Exception("Customer ID not found.")
        conn.commit()
        messagebox.showinfo("Success", "Customer deleted successfully.")
    except Exception as e:
        messagebox.showerror("Error", str(e))

crud_btn_frame = tk.Frame(crud_frame)
crud_btn_frame.grid(row=5, column=0, columnspan=2, pady=10)

tk.Button(crud_btn_frame, text="Add", command=add_customer).grid(row=0, column=0, padx=5)
tk.Button(crud_btn_frame, text="Update", command=update_customer).grid(row=0, column=1, padx=5)
tk.Button(crud_btn_frame, text="Delete", command=delete_customer).grid(row=0, column=2, padx=5)

side_frame = tk.Frame(root)
side_frame.pack(pady=10)

# Update Stock Section
stock_frame = tk.LabelFrame(side_frame, text="Update Disc Stock")
stock_frame.grid(row=0, column=0, padx=20)

tk.Label(stock_frame, text="Disc ID:").grid(row=0, column=0)
stock_disc_id_entry = tk.Entry(stock_frame)
stock_disc_id_entry.grid(row=0, column=1)

tk.Label(stock_frame, text="New Stock:").grid(row=1, column=0)
stock_value_entry = tk.Entry(stock_frame)
stock_value_entry.grid(row=1, column=1)

def update_stock():
    try:
        cursor.execute("UPDATE Discs SET stock=? WHERE disc_id=?",
                       (stock_value_entry.get(), stock_disc_id_entry.get()))
        if cursor.rowcount == 0:
            raise Exception("Disc ID not found.")
        conn.commit()
        messagebox.showinfo("Success", "Stock updated successfully.")
    except Exception as e:
        messagebox.showerror("Error", str(e))

tk.Button(stock_frame, text="Update Stock", command=update_stock).grid(row=2, column=0, columnspan=2, pady=5)

# Insert Order Section
order_frame = tk.LabelFrame(side_frame, text="Insert New Order")
order_frame.grid(row=0, column=1, padx=20)

tk.Label(order_frame, text="Customer ID:").grid(row=0, column=0)
order_customer_id = tk.Entry(order_frame)
order_customer_id.grid(row=0, column=1)

tk.Label(order_frame, text="Disc ID:").grid(row=1, column=0)
order_disc_id = tk.Entry(order_frame)
order_disc_id.grid(row=1, column=1)

tk.Label(order_frame, text="Order Date (YYYY-MM-DD):").grid(row=2, column=0)
order_date = tk.Entry(order_frame)
order_date.grid(row=2, column=1)

tk.Label(order_frame, text="Quantity:").grid(row=3, column=0)
order_quantity = tk.Entry(order_frame)
order_quantity.grid(row=3, column=1)

def insert_order():
    try:
        cursor.execute("INSERT INTO Orders (customer_id, disc_id, order_date, quantity) VALUES (?, ?, ?, ?)",
                       (order_customer_id.get(), order_disc_id.get(), order_date.get(), order_quantity.get()))
        conn.commit()
        messagebox.showinfo("Success", "Order inserted successfully.")
    except Exception as e:
        messagebox.showerror("Error", str(e))

tk.Button(order_frame, text="Insert Order", command=insert_order).grid(row=4, column=0, columnspan=2, pady=5)

root.mainloop()
conn.close()
