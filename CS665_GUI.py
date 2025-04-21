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
root.geometry("800x600")

# UI Components
query_output = tk.Text(root, height=15, width=100)
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

# Entry for custom query (optional)
query_entry = tk.Entry(root, width=100)
query_entry.pack(pady=5)
tk.Button(root, text="Run Custom SQL", command=lambda: run_query(query_entry.get())).pack()

root.mainloop()
conn.close()
