# **Factory Accounting System**

### **Main Purpose:**
A system to manage factories, employees, products, orders, inventory, accounting, with role-based access:

* Admin: System control, statistics, add factories and managers

* Manager: Factory operations, manages employees, orders, products, customers

### **Entities & Descriptions**

Entity | Description | Key Relationships |
---|---|---|
Admin | Controls system, adds factories and managers, views stats | 1 Admin → Many Factories
Factory | Represents a factory | 1 Factory → 1 Manager, 1 Factory → Many Employees / Products / Orders / Materials / Suppliers / Expense
Manager | Manages a factory | 1 Manager → 1 Factory, manages Employees, Orders, Products
Employee | Works in factory | Belongs to Factory
Customer | Places orders | 1 Customer → Many Orders
Supplier | Supplies materials | Many-to-Many with Materials (via SupplierMaterial)
Material | Raw materials | Many-to-Many with Suppliers (via SupplierMaterial), tracked in Inventory
Product | Manufactured goods | Belongs to Factory, part of OrderItem, tracked in Inventory
Order | Customer orders | 1 Order → Many OrderItems, generates Invoice
OrderItem | Line items in order | Links Product and Order
Invoice | Billing for order | 1 Invoice → 1 Order
Inventory | Tracks stock quantities | Linked to Product / Material
Vacation | Employee leave | Linked to Employee
Salary | Employee payroll | Linked to Employee
Expense | Tracks factory expenses | linked to Factory
User | Authentication & roles | Linked to Admin / Manager

### Admin Capabilities
* Create factories
* Assign manager to factory
* View system statistics:
  - Total Factories
  - Total Managers

### Relationships

