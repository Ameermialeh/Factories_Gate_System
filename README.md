# **Factory Accounting System**

### **Main Purpose:**
A system to manage factories, employees, products, orders, inventory, accounting, with role-based access:

* Admin: System control, statistics, add factories and managers

* Manager: Factory operations, manages employees, orders, products, customers

### **Entities & Descriptions**

Entity | Description | Key Relationships |
---|---|---|
Admin | Controls system, adds factories and managers, views stats | (1) Admin → (Many) Factories
Factory | Represents a factory | (1) Factory → (1) Manager, (1) Factory → (Many) Employees / Products / Orders / Materials / Suppliers
Manager | Manages a factory | (1) Manager → (1) Factory, manages Employees, Orders, Products, Materials, Suppliers and Expense
Employee | Works in factory | Belongs to Factory, 
Customer | Places orders | (1) Customer → (Many) Orders
Supplier | Supplies materials | (Many-to-Many) with Materials (via MaterialPurchase)
Material | Raw materials | (Many-to-Many) with Suppliers (via MaterialPurchase), tracked in Inventory
Product | Manufactured goods | Belongs to Factory, part of OrderItem, tracked in Inventory
Order | Customer orders | (1) Order → (Many) OrderItems, generates Invoice
OrderItem | Line items in order | Links Product and Order
Invoice | Billing for order | (1) Invoice → (1) Order
Inventory | Tracks stock quantities | (1) Inventory → (many) Product / Material
Vacation | Employee leave | (1) Employee → (many) vacations
Salary | Employee payroll | (1) Employee → (many) Monthly salary
Expense | Tracks factory expenses | linked to Factory

### Admin Capabilities
* Create factories
* Assign manager to factory
* View system statistics:
  - Total Factories
  - Total Managers

### UML Diagram:
![alt text](https://github.com/Ameermialeh/Factories_Gate_System/blob/main/UML_Diagram/Factories_Managment_System_UML.jpg)

