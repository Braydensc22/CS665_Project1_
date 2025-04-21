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
root.geometry("900x700")

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

# Example Queries (Adjust table/columns based on your schema)
def show_all_customers():
    run_query("SELECT * FROM Customers")

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
tk.Button(btn_frame, text="Orders with Customer Names", command=show_orders_with_customer_names).grid(row=0, column=1, padx=5, pady=5)
tk.Button(btn_frame, text="Most Popular Disc", command=show_most_popular_disc).grid(row=0, column=2, padx=5, pady=5)

# Custom SQL
query_entry = tk.Entry(root, width=100)
query_entry.pack(pady=5)
tk.Button(root, text="Run Custom SQL", command=lambda: run_query(query_entry.get())).pack(pady=5)

# CRUD Operations
crud_frame = tk.LabelFrame(root, text="Customer Management")
crud_frame.pack(pady=10)

# Entries
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

# Add Customer

def add_customer():
    try:
        cursor.execute("INSERT INTO Customers (first_name, last_name, email, phone) VALUES (?, ?, ?, ?)",
                       (first_name_entry.get(), last_name_entry.get(), email_entry.get(), phone_entry.get()))
        conn.commit()
        messagebox.showinfo("Success", "Customer added successfully.")
    except Exception as e:
        messagebox.showerror("Error", str(e))

# Update Customer

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

# Delete Customer

def delete_customer():
    try:
        cursor.execute("DELETE FROM Customers WHERE customer_id=?", (customer_id_entry.get(),))
        if cursor.rowcount == 0:
            raise Exception("Customer ID not found.")
        conn.commit()
        messagebox.showinfo("Success", "Customer deleted successfully.")
    except Exception as e:
        messagebox.showerror("Error", str(e))

# Buttons for CRUD
crud_btn_frame = tk.Frame(crud_frame)
crud_btn_frame.grid(row=5, column=0, columnspan=2, pady=10)

tk.Button(crud_btn_frame, text="Add", command=add_customer).grid(row=0, column=0, padx=5)
tk.Button(crud_btn_frame, text="Update", command=update_customer).grid(row=0, column=1, padx=5)
tk.Button(crud_btn_frame, text="Delete", command=delete_customer).grid(row=0, column=2, padx=5)

root.mainloop()
conn.close()
