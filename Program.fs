open System
open System.Data
open System.Data.SQLite
open System.Windows.Forms
open System.Drawing

// Main application module
module DiscInventoryApp =
    // Database connection
    let connectionString = "Data Source=Create.sqlite;Version=3;"
    
    // Helper function to execute queries and return DataTable
    let executeQuery (query: string) =
        use connection = new SQLiteConnection(connectionString)
        connection.Open()
        use command = new SQLiteCommand(query, connection)
        use adapter = new SQLiteDataAdapter(command)
        let dataTable = new DataTable()
        adapter.Fill(dataTable) |> ignore
        dataTable
        
    // Execute non-query commands (INSERT, UPDATE, DELETE)
    let executeNonQuery (query: string) =
        use connection = new SQLiteConnection(connectionString)
        connection.Open()
        use command = new SQLiteCommand(query, connection)
        command.ExecuteNonQuery()
        
    // Create the main form
    let createMainForm() =
        let form = new Form(
                        Text = "Disc Golf Store Management System",
                        Size = Size(1200, 800),
                        StartPosition = FormStartPosition.CenterScreen)
                        
        // Create DataGridView to display results
        let dataGridView = new DataGridView(
                                Dock = DockStyle.Fill,
                                AllowUserToAddRows = false,
                                AllowUserToDeleteRows = false,
                                ReadOnly = true,
                                MultiSelect = false,
                                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill)
        
        // Create tabs for different operations
        let tabControl = new TabControl(Dock = DockStyle.Fill)
        
        // Create tab pages for different operations
        let tabView = new TabPage("View Data")
        let tabCrud = new TabPage("CRUD Operations")
        let tabQueries = new TabPage("Advanced Queries")
        let tabReports = new TabPage("Reports")
        
        tabControl.TabPages.Add(tabView)
        tabControl.TabPages.Add(tabCrud)
        tabControl.TabPages.Add(tabQueries)
        tabControl.TabPages.Add(tabReports)
        
        // VIEW TAB
        // Create panel for view buttons
        let viewButtonPanel = new FlowLayoutPanel(
                                Dock = DockStyle.Top,
                                AutoSize = true,
                                FlowDirection = FlowDirection.LeftToRight,
                                WrapContents = true,
                                Padding = Padding(10))
                                
        // Add a DataGridView for the View tab
        let viewDataGridView = new DataGridView(
                                  Dock = DockStyle.Fill,
                                  AllowUserToAddRows = false,
                                  AllowUserToDeleteRows = false,
                                  ReadOnly = true,
                                  MultiSelect = false,
                                  SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                                  AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill)
                                
        let createViewButton (text: string) (query: string) =
            let button = new Button(Text = text, Width = 150, Height = 40, Margin = Padding(5))
            button.Click.Add(fun _ -> 
                try
                    viewDataGridView.DataSource <- executeQuery query
                with ex ->
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
            )
            button
            
        let viewDiscButton = createViewButton "View All Discs" "SELECT * FROM Discs"
        let viewBrandsButton = createViewButton "View All Brands" "SELECT * FROM Brands"
        let viewCustomersButton = createViewButton "View All Customers" "SELECT * FROM Customers"
        let viewOrdersButton = createViewButton "View All Orders" "SELECT * FROM Orders"
        
        viewButtonPanel.Controls.AddRange([| viewDiscButton; viewBrandsButton; viewCustomersButton; viewOrdersButton |])
        
        // CRUD TAB
        // Create panels for CRUD operations
        let crudPanel = new TableLayoutPanel(
                            Dock = DockStyle.Fill,
                            ColumnCount = 2,
                            RowCount = 1)
        
        let leftPanel = new Panel(Dock = DockStyle.Fill, Padding = Padding(10))
        let rightPanel = new Panel(Dock = DockStyle.Fill, Padding = Padding(10))
        
        crudPanel.Controls.Add(leftPanel, 0, 0)
        crudPanel.Controls.Add(rightPanel, 1, 0)
        
        // CRUD DataGridView
        let crudDataGridView = new DataGridView(
                                 Dock = DockStyle.Bottom,
                                 Height = 300,
                                 AllowUserToAddRows = false,
                                 AllowUserToDeleteRows = false,
                                 ReadOnly = true,
                                 MultiSelect = false,
                                 SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                                 AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill)
        
        // Create ComboBox for table selection
        let tableLabel = new Label(Text = "Select Table:", AutoSize = true)
        let tableComboBox = new ComboBox(
                                DropDownStyle = ComboBoxStyle.DropDownList,
                                Width = 200)
        tableComboBox.Items.AddRange([| "Discs"; "Brands"; "Customers"; "Orders" |])
        tableComboBox.SelectedIndex <- 0
        
        // Form for adding/updating records
        let formPanel = new FlowLayoutPanel(
                            FlowDirection = FlowDirection.TopDown,
                            AutoSize = true,
                            Width = 400)
        
        // Disc fields
        let discIdLabel = new Label(Text = "Disc ID:", Width = 100)
        let discIdTextBox = new TextBox(Width = 250)
        let discNameLabel = new Label(Text = "Name:", Width = 100)
        let discNameTextBox = new TextBox(Width = 250)
        let brandIdLabel = new Label(Text = "Brand ID:", Width = 100)
        let brandIdTextBox = new TextBox(Width = 250)
        let typeLabel = new Label(Text = "Type:", Width = 100)
        let typeComboBox = new ComboBox(Width = 250, DropDownStyle = ComboBoxStyle.DropDownList)
        typeComboBox.Items.AddRange([| "Driver"; "Midrange"; "Putter" |])
        let priceLabel = new Label(Text = "Price ($):", Width = 100)
        let priceTextBox = new TextBox(Width = 250)
        let weightLabel = new Label(Text = "Weight (g):", Width = 100)
        let weightTextBox = new TextBox(Width = 250)
        let speedLabel = new Label(Text = "Speed (1-14):", Width = 100)
        let speedTextBox = new TextBox(Width = 250)
        let glideLabel = new Label(Text = "Glide (1-7):", Width = 100)
        let glideTextBox = new TextBox(Width = 250)
        let turnLabel = new Label(Text = "Turn (-5-1):", Width = 100)
        let turnTextBox = new TextBox(Width = 250)
        let fadeLabel = new Label(Text = "Fade (0-5):", Width = 100)
        let fadeTextBox = new TextBox(Width = 250)
        let stockLabel = new Label(Text = "Stock:", Width = 100)
        let stockTextBox = new TextBox(Width = 250)
        
        // Brand fields
        let brandNameLabel = new Label(Text = "Name:", Width = 100)
        let brandNameTextBox = new TextBox(Width = 250)
        let cityLabel = new Label(Text = "City:", Width = 100)
        let cityTextBox = new TextBox(Width = 250)
        let countryLabel = new Label(Text = "Country:", Width = 100)
        let countryTextBox = new TextBox(Width = 250)
        let establishedLabel = new Label(Text = "Est. Year:", Width = 100)
        let establishedTextBox = new TextBox(Width = 250)
        
        // Customer fields
        let customerIdLabel = new Label(Text = "Customer ID:", Width = 100)
        let customerIdTextBox = new TextBox(Width = 250)
        let firstNameLabel = new Label(Text = "First Name:", Width = 100)
        let firstNameTextBox = new TextBox(Width = 250)
        let lastNameLabel = new Label(Text = "Last Name:", Width = 100)
        let lastNameTextBox = new TextBox(Width = 250)
        let emailLabel = new Label(Text = "Email:", Width = 100)
        let emailTextBox = new TextBox(Width = 250)
        let phoneLabel = new Label(Text = "Phone:", Width = 100)
        let phoneTextBox = new TextBox(Width = 250)
        
        // Order fields
        let orderIdLabel = new Label(Text = "Order ID:", Width = 100)
        let orderIdTextBox = new TextBox(Width = 250)
        let orderCustomerIdLabel = new Label(Text = "Customer ID:", Width = 100)
        let orderCustomerIdTextBox = new TextBox(Width = 250)
        let orderDiscIdLabel = new Label(Text = "Disc ID:", Width = 100)
        let orderDiscIdTextBox = new TextBox(Width = 250)
        let orderDateLabel = new Label(Text = "Order Date:", Width = 100)
        let orderDatePicker = new DateTimePicker(Width = 250, Format = DateTimePickerFormat.Short)
        let quantityLabel = new Label(Text = "Quantity:", Width = 100)
        let quantityTextBox = new TextBox(Width = 250)
        
        // Add disc form fields initially
        formPanel.Controls.AddRange([| 
            discIdLabel; discIdTextBox;
            discNameLabel; discNameTextBox;
            brandIdLabel; brandIdTextBox;
            typeLabel; typeComboBox;
            priceLabel; priceTextBox;
            weightLabel; weightTextBox;
            speedLabel; speedTextBox;
            glideLabel; glideTextBox;
            turnLabel; turnTextBox;
            fadeLabel; fadeTextBox;
            stockLabel; stockTextBox
        |])
        
        // CRUD Buttons
        let crudButtonPanel = new FlowLayoutPanel(
                                FlowDirection = FlowDirection.LeftToRight,
                                AutoSize = true,
                                Dock = DockStyle.Top,
                                Padding = Padding(10))
                                
        let addButton = new Button(Text = "Add", Width = 100, Height = 40, Margin = Padding(5))
        let updateButton = new Button(Text = "Update", Width = 100, Height = 40, Margin = Padding(5))
        let deleteButton = new Button(Text = "Delete", Width = 100, Height = 40, Margin = Padding(5))
        let clearButton = new Button(Text = "Clear Fields", Width = 100, Height = 40, Margin = Padding(5))
        
        // Clear all form fields
        let clearDiscFields() =
            discIdTextBox.Text <- ""
            discNameTextBox.Text <- ""
            brandIdTextBox.Text <- ""
            typeComboBox.SelectedIndex <- -1
            priceTextBox.Text <- ""
            weightTextBox.Text <- ""
            speedTextBox.Text <- ""
            glideTextBox.Text <- ""
            turnTextBox.Text <- ""
            fadeTextBox.Text <- ""
            stockTextBox.Text <- ""
            
        let clearBrandFields() =
            brandNameTextBox.Text <- ""
            cityTextBox.Text <- ""
            countryTextBox.Text <- ""
            establishedTextBox.Text <- ""
            
        let clearCustomerFields() =
            customerIdTextBox.Text <- ""
            firstNameTextBox.Text <- ""
            lastNameTextBox.Text <- ""
            emailTextBox.Text <- ""
            phoneTextBox.Text <- ""
            
        let clearOrderFields() =
            orderIdTextBox.Text <- ""
            orderCustomerIdTextBox.Text <- ""
            orderDiscIdTextBox.Text <- ""
            orderDatePicker.Value <- DateTime.Now
            quantityTextBox.Text <- ""
            
        let clearFields() =
            match tableComboBox.SelectedItem.ToString() with
            | "Discs" -> clearDiscFields()
            | "Brands" -> clearBrandFields()
            | "Customers" -> clearCustomerFields()
            | "Orders" -> clearOrderFields()
            | _ -> ()
        
        // Update form based on selected table
        let updateFormForTable() =
            formPanel.Controls.Clear()
            
            match tableComboBox.SelectedItem.ToString() with
            | "Discs" ->
                formPanel.Controls.AddRange([| 
                    discIdLabel; discIdTextBox;
                    discNameLabel; discNameTextBox;
                    brandIdLabel; brandIdTextBox;
                    typeLabel; typeComboBox;
                    priceLabel; priceTextBox;
                    weightLabel; weightTextBox;
                    speedLabel; speedTextBox;
                    glideLabel; glideTextBox;
                    turnLabel; turnTextBox;
                    fadeLabel; fadeTextBox;
                    stockLabel; stockTextBox
                |])
                clearDiscFields()
            | "Brands" ->
                formPanel.Controls.AddRange([|
                    brandIdLabel; brandIdTextBox;
                    brandNameLabel; brandNameTextBox;
                    cityLabel; cityTextBox;
                    countryLabel; countryTextBox;
                    establishedLabel; establishedTextBox
                |])
                clearBrandFields()
            | "Customers" ->
                formPanel.Controls.AddRange([|
                    customerIdLabel; customerIdTextBox;
                    firstNameLabel; firstNameTextBox;
                    lastNameLabel; lastNameTextBox;
                    emailLabel; emailTextBox;
                    phoneLabel; phoneTextBox
                |])
                clearCustomerFields()
            | "Orders" ->
                formPanel.Controls.AddRange([|
                    orderIdLabel; orderIdTextBox;
                    orderCustomerIdLabel; orderCustomerIdTextBox;
                    orderDiscIdLabel; orderDiscIdTextBox;
                    orderDateLabel; orderDatePicker;
                    quantityLabel; quantityTextBox
                |])
                clearOrderFields()
            | _ -> ()
            
            formPanel.Refresh()
        
        // Add button click event
        addButton.Click.Add(fun _ ->
            try
                match tableComboBox.SelectedItem.ToString() with
                | "Discs" ->
                    let query = sprintf "INSERT INTO Discs (name, brand_id, type, speed, glide, turn, fade, price, stock) VALUES ('%s', %s, '%s', %s, %s, %s, %s, %s, %s)"
                                discNameTextBox.Text brandIdTextBox.Text typeComboBox.SelectedItem.ToString() 
                                speedTextBox.Text glideTextBox.Text turnTextBox.Text fadeTextBox.Text
                                priceTextBox.Text stockTextBox.Text
                    let result = executeNonQuery query
                    MessageBox.Show(sprintf "Added %d record(s)" result, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
                    clearDiscFields()
                | "Brands" ->
                    let query = sprintf "INSERT INTO Brands (name, city, country, established_year) VALUES ('%s', '%s', '%s', %s)"
                                brandNameTextBox.Text cityTextBox.Text countryTextBox.Text establishedTextBox.Text
                    let result = executeNonQuery query
                    MessageBox.Show(sprintf "Added %d record(s)" result, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
                    clearBrandFields()
                | "Customers" ->
                    let query = sprintf "INSERT INTO Customers (first_name, last_name, email, phone) VALUES ('%s', '%s', '%s', '%s')"
                                firstNameTextBox.Text lastNameTextBox.Text emailTextBox.Text phoneTextBox.Text
                    let result = executeNonQuery query
                    MessageBox.Show(sprintf "Added %d record(s)" result, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
                    clearCustomerFields()
                | "Orders" ->
                    let dateString = orderDatePicker.Value.ToString("yyyy-MM-dd")
                    let query = sprintf "INSERT INTO Orders (customer_id, disc_id, order_date, quantity) VALUES (%s, %s, '%s', %s)"
                                orderCustomerIdTextBox.Text orderDiscIdTextBox.Text dateString quantityTextBox.Text
                    let result = executeNonQuery query
                    MessageBox.Show(sprintf "Added %d record(s)" result, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
                    clearOrderFields()
                | _ -> ()
                
                // Refresh data grid
                crudDataGridView.DataSource <- executeQuery (sprintf "SELECT * FROM %s" (tableComboBox.SelectedItem.ToString()))
            with ex ->
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        )
        
        // Update button click event
        updateButton.Click.Add(fun _ ->
            try
                match tableComboBox.SelectedItem.ToString() with
                | "Discs" when not (String.IsNullOrEmpty(discIdTextBox.Text)) ->
                    let query = sprintf "UPDATE Discs SET name = '%s', brand_id = %s, type = '%s', speed = %s, glide = %s, turn = %s, fade = %s, price = %s, stock = %s WHERE disc_id = %s"
                                discNameTextBox.Text brandIdTextBox.Text typeComboBox.SelectedItem.ToString() 
                                speedTextBox.Text glideTextBox.Text turnTextBox.Text fadeTextBox.Text
                                priceTextBox.Text stockTextBox.Text discIdTextBox.Text
                    let result = executeNonQuery query
                    MessageBox.Show(sprintf "Updated %d record(s)" result, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
                    
                | "Brands" when not (String.IsNullOrEmpty(brandIdTextBox.Text)) ->
                    let query = sprintf "UPDATE Brands SET name = '%s', city = '%s', country = '%s', established_year = %s WHERE brand_id = %s"
                                brandNameTextBox.Text cityTextBox.Text countryTextBox.Text establishedTextBox.Text brandIdTextBox.Text
                    let result = executeNonQuery query
                    MessageBox.Show(sprintf "Updated %d record(s)" result, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
                    
                | "Customers" when not (String.IsNullOrEmpty(customerIdTextBox.Text)) ->
                    let query = sprintf "UPDATE Customers SET first_name = '%s', last_name = '%s', email = '%s', phone = '%s' WHERE customer_id = %s"
                                firstNameTextBox.Text lastNameTextBox.Text emailTextBox.Text phoneTextBox.Text customerIdTextBox.Text
                    let result = executeNonQuery query
                    MessageBox.Show(sprintf "Updated %d record(s)" result, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
                    
                | "Orders" when not (String.IsNullOrEmpty(orderIdTextBox.Text)) ->
                    let dateString = orderDatePicker.Value.ToString("yyyy-MM-dd")
                    let query = sprintf "UPDATE Orders SET customer_id = %s, disc_id = %s, order_date = '%s', quantity = %s WHERE order_id = %s"
                                orderCustomerIdTextBox.Text orderDiscIdTextBox.Text dateString quantityTextBox.Text orderIdTextBox.Text
                    let result = executeNonQuery query
                    MessageBox.Show(sprintf "Updated %d record(s)" result, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
                    
                | _ ->
                    MessageBox.Show("Please select a record to update", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
                
                // Refresh data grid
                crudDataGridView.DataSource <- executeQuery (sprintf "SELECT * FROM %s" (tableComboBox.SelectedItem.ToString()))
            with ex ->
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        )
        
        // Delete button click event
        deleteButton.Click.Add(fun _ ->
            try
                let idToDelete, tableName, idField =
                    match tableComboBox.SelectedItem.ToString() with
                    | "Discs" -> discIdTextBox.Text, "Discs", "disc_id"
                    | "Brands" -> brandIdTextBox.Text, "Brands", "brand_id"
                    | "Customers" -> customerIdTextBox.Text, "Customers", "customer_id"
                    | "Orders" -> orderIdTextBox.Text, "Orders", "order_id"
                    | _ -> "", "", ""
                
                if not (String.IsNullOrEmpty(idToDelete)) then
                    if MessageBox.Show("Are you sure you want to delete this record?", "Confirm", 
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes then
                        let query = sprintf "DELETE FROM %s WHERE %s = %s" tableName idField idToDelete
                        let result = executeNonQuery query
                        MessageBox.Show(sprintf "Deleted %d record(s)" result, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
                        clearFields()
                        crudDataGridView.DataSource <- executeQuery (sprintf "SELECT * FROM %s" (tableComboBox.SelectedItem.ToString()))
                else
                    MessageBox.Show("Please select a record to delete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
            with ex ->
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        )
        
        // Clear button click event
        clearButton.Click.Add(fun _ -> clearFields())
        
        // Handle grid cell click to populate form fields
        crudDataGridView.CellClick.Add(fun e ->
            if e.RowIndex >= 0 then
                try
                    let selectedRow = crudDataGridView.Rows.[e.RowIndex]
                    
                    match tableComboBox.SelectedItem.ToString() with
                    | "Discs" ->
                        discIdTextBox.Text <- selectedRow.Cells.["disc_id"].Value.ToString()
                        discNameTextBox.Text <- selectedRow.Cells.["name"].Value.ToString()
                        brandIdTextBox.Text <- selectedRow.Cells.["brand_id"].Value.ToString()
                        
                        // Set type in combobox
                        let typeValue = selectedRow.Cells.["type"].Value.ToString()
                        for i in 0 .. typeComboBox.Items.Count - 1 do
                            if typeComboBox.Items.[i].ToString() = typeValue then
                                typeComboBox.SelectedIndex <- i
                                
                        speedTextBox.Text <- selectedRow.Cells.["speed"].Value.ToString()
                        glideTextBox.Text <- selectedRow.Cells.["glide"].Value.ToString()
                        turnTextBox.Text <- selectedRow.Cells.["turn"].Value.ToString()
                        fadeTextBox.Text <- selectedRow.Cells.["fade"].Value.ToString()
                        priceTextBox.Text <- selectedRow.Cells.["price"].Value.ToString()
                        stockTextBox.Text <- selectedRow.Cells.["stock"].Value.ToString()
                        
                    | "Brands" ->
                        brandIdTextBox.Text <- selectedRow.Cells.["brand_id"].Value.ToString()
                        brandNameTextBox.Text <- selectedRow.Cells.["name"].Value.ToString()
                        cityTextBox.Text <- selectedRow.Cells.["city"].Value.ToString()
                        countryTextBox.Text <- selectedRow.Cells.["country"].Value.ToString()
                        establishedTextBox.Text <- selectedRow.Cells.["established_year"].Value.ToString()
                        
                    | "Customers" ->
                        customerIdTextBox.Text <- selectedRow.Cells.["customer_id"].Value.ToString()
                        firstNameTextBox.Text <- selectedRow.Cells.["first_name"].Value.ToString()
                        lastNameTextBox.Text <- selectedRow.Cells.["last_name"].Value.ToString()
                        emailTextBox.Text <- selectedRow.Cells.["email"].Value.ToString()
                        phoneTextBox.Text <- selectedRow.Cells.["phone"].Value.ToString()
                        
                    | "Orders" ->
                        orderIdTextBox.Text <- selectedRow.Cells.["order_id"].Value.ToString()
                        orderCustomerIdTextBox.Text <- selectedRow.Cells.["customer_id"].Value.ToString()
                        orderDiscIdTextBox.Text <- selectedRow.Cells.["disc_id"].Value.ToString()
                        
                        // Parse date
                        try
                            let dateValue = selectedRow.Cells.["order_date"].Value.ToString()
                            orderDatePicker.Value <- DateTime.Parse(dateValue)
                        with _ ->
                            orderDatePicker.Value <- DateTime.Now
                            
                        quantityTextBox.Text <- selectedRow.Cells.["quantity"].Value.ToString()
                        
                    | _ -> ()
                with ex ->
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        )
        
        // Update grid when table selection changes
        tableComboBox.SelectedIndexChanged.Add(fun _ ->
            updateFormForTable()
            let tableName = tableComboBox.SelectedItem.ToString()
            try
                crudDataGridView.DataSource <- executeQuery (sprintf "SELECT * FROM %s" tableName)
            with ex ->
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        )
        
        crudButtonPanel.Controls.AddRange([| addButton; updateButton; deleteButton; clearButton |])
        
        // Organize CRUD panel
        let controlPanel = new FlowLayoutPanel(
                                FlowDirection = FlowDirection.TopDown,
                                AutoSize = true,
                                Dock = DockStyle.Top)
        controlPanel.Controls.Add(tableLabel)
        controlPanel.Controls.Add(tableComboBox)
        
        leftPanel.Controls.Add(controlPanel)
        leftPanel.Controls.Add(crudButtonPanel)
        rightPanel.Controls.Add(formPanel)
        
        tabCrud.Controls.Add(crudDataGridView)
        tabCrud.Controls.Add(crudPanel)
        
        // ADVANCED QUERIES TAB
        // Create panel for advanced query buttons
        let advQueryPanel = new FlowLayoutPanel(
                                Dock = DockStyle.Top,
                                AutoSize = true,
                                FlowDirection = FlowDirection.LeftToRight,
                                WrapContents = true,
                                Padding = Padding(10))
        
        // DataGridView for displaying query results
        let queryDataGridView = new DataGridView(
                                   Dock = DockStyle.Fill,
                                   AllowUserToAddRows = false,
                                   AllowUserToDeleteRows = false,
                                   ReadOnly = true,
                                   MultiSelect = false,
                                   SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                                   AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill)
                                   
        let createQueryButton (text: string) (query: string) =
            let button = new Button(Text = text, Width = 220, Height = 60, Margin = Padding(5))
            button.Click.Add(fun _ -> 
                try
                    queryDataGridView.DataSource <- executeQuery query
                with ex ->
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
            )
            button
            
        // Create advanced query buttons
        let discsByBrandBtn = createQueryButton "Discs by Brand (JOIN)" 
                               """SELECT d.disc_id, d.name as 'Disc Name', b.name as 'Brand', 
                                         d.type, d.speed, d.glide, d.turn, d.fade, 
                                         d.price, d.stock 
                                  FROM Discs d
                                  JOIN Brands b ON d.brand_id = b.brand_id
                                  ORDER BY b.name, d.name"""
                               
        let orderDetailsBtn = createQueryButton "Order Details (JOIN)" 
                               """SELECT o.order_id, c.first_name || ' ' || c.last_name as 'Customer', 
                                         d.name as 'Disc', b.name as 'Brand', 
                                         o.order_date, o.quantity, 
                                         o.quantity * d.price as 'Total Price'
                                  FROM Orders o
                                  JOIN Customers c ON o.customer_id = c.customer_id
                                  JOIN Discs d ON o.disc_id = d.disc_id
                                  JOIN Brands b ON d.brand_id = b.brand_id
                                  ORDER BY o.order_date DESC"""
                                  
        let lowStockBtn = createQueryButton "Low Stock Alert (<5)" 
                           """SELECT d.disc_id, d.name, b.name as 'Brand', 
                                     d.type, d.price, d.stock
                              FROM Discs d
                              JOIN Brands b ON d.brand_id = b.brand_id
                              WHERE d.stock < 5
                              ORDER BY d.stock"""
                              
        let topCustomersBtn = createQueryButton "Top Customers (Subquery)" 
                              """SELECT c.customer_id, c.first_name || ' ' || c.last_name as 'Customer', 
                                        c.email, c.phone,
                                        (SELECT COUNT(*) FROM Orders o WHERE o.customer_id = c.customer_id) as 'Orders',
                                        (SELECT SUM(o.quantity) FROM Orders o WHERE o.customer_id = c.customer_id) as 'Total Items'
                                 FROM Customers c
                                 ORDER BY 'Total Items' DESC"""
                                 
        let discStatsBtn = createQueryButton "Disc Stats by Type (Group By)" 
                            """SELECT d.type, COUNT(*) as 'Count', 
                                      AVG(d.price) as 'Avg Price',
                                      AVG(d.speed) as 'Avg Speed', 
                                      AVG(d.glide) as 'Avg Glide',
                                      AVG(d.turn) as 'Avg Turn',
                                      AVG(d.fade) as 'Avg Fade'
                               FROM Discs d
                               GROUP BY d.type
                               ORDER BY 'Count' DESC"""
        
        let inventoryValueBtn = createQueryButton "Total Inventory Value (Aggregate)" 
                                """SELECT b.name as 'Brand',
                                          COUNT(d.disc_id) as 'Disc Count',
                                          SUM(d.stock)
                                          