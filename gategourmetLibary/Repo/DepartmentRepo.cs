using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gategourmetLibrary.Models;
using gategourmetLibrary.Service;
using Microsoft.Data.SqlClient;

namespace gategourmetLibrary.Repo
{
    public class DepartmentRepo : IDepartmentRepo
    {
        // connection string bruges til at kommuniker med database 
        private readonly string _connectionString;

        // constructor modtager connection string fra service 
        public DepartmentRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        // returns all departments
        public List<Department> GetAllDepartments()
        {
            List<Department> departments = new List<Department>();
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(
                "SELECT D_ID, D_Name, D_Location, D_Email FROM Department",
                connection);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            // gennemgår hver department fra hver table 
            while (reader.Read())
            {
                // der laves et objekt for hver række 
                Department department = new Department();
                department.DepartmentId = (int)reader["D_ID"];
                department.DepartmentName = reader["D_Name"].ToString();
                department.DepartmentLocation = reader["D_Location"].ToString();
                department.DepartmentEmail = reader["email"].ToString();

                departments.Add(department);
            }

            reader.Close();
            connection.Close();

            return departments;
        }
        // adds a new department to database 
        public void AddDepartment(Department newDepartment)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(
                "INSERT INTO Department (D_Name, D_Location, D_Email)" +
                "VALUES (@name, @location, @mail)",
                connection);

            command.Parameters.AddWithValue("@name", newDepartment.DepartmentName);
            command.Parameters.AddWithValue("@location", newDepartment.DepartmentLocation);
            command.Parameters.AddWithValue("@mail", newDepartment.DepartmentEmail);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

        }
        // deletes a department with matching ID
        public void DeleteDepartment(int departmentId)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            SqlCommand command = new SqlCommand(
               "DELETE FROM Departments WHERE D_ID = @id",
               connection);

            command.Parameters.AddWithValue("@id", departmentId);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

        }
        // updates department info by ID
        public void UpdateDepartment(int departmentId, Department updatedDepartment)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            SqlCommand command = new SqlCommand(
                "UPDATE Department SET D_Name =@name,D_Location=@location,D_Email=@mail" +
                "Where D_ID = @id",
                connection);

            command.Parameters.AddWithValue("@name", updatedDepartment.DepartmentName);
            command.Parameters.AddWithValue("@location", updatedDepartment.DepartmentLocation);
            command.Parameters.AddWithValue("@mail", updatedDepartment.DepartmentEmail);
            command.Parameters.AddWithValue("@id", updatedDepartment.DepartmentId);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        // returns a specific department by ID
        public Department GetDepartment(int departmentId)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(
                "SELECT D_ID, D_Name, D_Location, D_Email FROM Department WHERE D_ID =@id",
                connection);

            command.Parameters.AddWithValue("@id", departmentId);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Department department = new Department();
                department.DepartmentId = (int)reader["D_ID"];
                department.DepartmentName = reader["D_Name"].ToString();
                department.DepartmentLocation = reader["D_Location"].ToString();
                department.DepartmentEmail = reader["D_Email"].ToString();

                reader.Close();
                connection.Close();
                return department;
            }
            reader.Close();
            connection.Close();
            return null;
        }


        // assigns a new warehouse to a department
        public void NewWarehouse(Warehouse newWarehouse)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            connection.Open();

            // indsætter warehouse og retunerer generet W_id
            SqlCommand insertWarehouse = new SqlCommand(
                "INSERT INTO Warehouse (W_Name, W_Type, W_Location)" +
                "VALUES (@name, @type,@location); SELECT SCOPE_IDENTITY();",
                connection);

            insertWarehouse.Parameters.AddWithValue("@name", newWarehouse.Name);
            insertWarehouse.Parameters.AddWithValue("@type", newWarehouse.Type);
            insertWarehouse.Parameters.AddWithValue("@location", newWarehouse.Location);

            object result = insertWarehouse.ExecuteScalar();
            int warehouseId = Convert.ToInt32(result);

            // linker warehouse til department 
            SqlCommand link = new SqlCommand(
                "INSERT INTO werehouseDepartment (D_ID, W_ID) VALUES (@d, @w)",
                connection);

            link.Parameters.AddWithValue("@d", newWarehouse.DepartmentId);
            link.Parameters.AddWithValue("@w", warehouseId);
            link.ExecuteNonQuery();

            connection.Close();

        }
        // stocks an ingredient in the department's warehouse
        public void StockIngredient(Ingredient stockIngredient)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            // opdater antal af ingredienser 
            SqlCommand updateQuantity = new SqlCommand(
                "UPDATE Ingredient SET I_quntity = I_quntity + @amount WHERE I_ID =@id",
                connection);

            updateQuantity.Parameters.AddWithValue("@amount", stockIngredient.Quantity);
            updateQuantity.Parameters.AddWithValue("@id", stockIngredient.ID);
            updateQuantity.ExecuteNonQuery();

            SqlCommand linkCheck = new SqlCommand(
                "SELECT COUNT (*) FROM werehouseIngredient WHERE W_ID = @w AND I_ID = @i", connection);

            linkCheck.Parameters.AddWithValue("@w", stockIngredient.WarehouseId);
            linkCheck.Parameters.AddWithValue("@i", stockIngredient.ID);

            int exists = Convert.ToInt32(linkCheck.ExecuteScalar());

            if (exists == 0)
            {
                SqlCommand createLink = new SqlCommand(
                    "INSERT INTO werehouseIngredient (W_ID, I_ID) VALUES (@w,@i)",
                    connection);

                createLink.Parameters.AddWithValue("@w", stockIngredient.WarehouseId);
                createLink.Parameters.AddWithValue("@i", stockIngredient.ID);
                createLink.ExecuteNonQuery();

            }
            connection.Close();
        }
        // gets the stock of a specific warehouse
        public List<Ingredient> GetWarehouseStock(int warehouseId)
        {
            // liste der indeholder alle ingredienser i warehouse 
            List<Ingredient> ingredients = new List<Ingredient>();

            SqlConnection connection = new SqlConnection(_connectionString);

            // sql mellem ingredient og werehouseingredient 
            SqlCommand command = new SqlCommand(
                "SELECT i.ID, i.I_Name,I.I_expireDate, i.I_quntity" +
                "FROM werehouseIngredient wi" +
                "JOIN Ingredient i ON wi.I_ID = i.I_ID" +
                "WHERE wi.W_ID = @id",
                connection);

            command.Parameters.AddWithValue("@id", warehouseId);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            // læser alle ingredienser for warehouse 
            while (reader.Read())
            {
                Ingredient ingredient = new Ingredient();
                ingredient.ID = (int)reader["I_ID"];
                ingredient.Name = reader["I_Name"].ToString();
                ingredient.ExpireDate = (DateTime)reader["I_expireDate"];
                ingredient.Quantity = (int)reader["I_quintity"];
                ingredient.WarehouseId = warehouseId;

                ingredients.Add(ingredient);
            }
            reader.Close();
            connection.Close();

            return ingredients;
        }
        // gets the managers of a specific department
        public List<Manager> GetDepartmentManagers(int departmentId)
        {
            return null;
        }
        // gets the employees of a specific department
        public List<Employee> GetDepartmentEmployees(int departmentId)
        {
            List<Employee> employees = new List<Employee>();

            SqlConnection connection = new SqlConnection(_connectionString);


            SqlCommand command = new SqlCommand(
                "SELECT e.Employee_ID, e.E_Name, e.E_Email, e.E_PhoneNumber" +
                "FROM EmployeeDepartment ed" +
                "JOIN Employee e ON ed.E_ID = e.Employee_ID" +
                "WHERE ed.D_ID = @id",
                connection);
            command.Parameters.AddWithValue("@id", departmentId);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            // læser alle medarbejder i denne afdeling 
            while (reader.Read())
            {
                Employee employee = new Employee();
                employee.Id = (int)reader["Employee_ID"];
                employee.Name = reader["E_Name"].ToString();
                employee.Email = reader["E_Email"].ToString();
                employee.PhoneNumber = reader["E_PhoneNumber"].ToString();

                employees.Add(employee);
            }
            reader.Close();
            connection.Close();

            return employees;
        }
        // adds a new manager to a department
        public void AddNewDepartmentManager(int departmentId, Manager newManager)
        {
            // vi har ikke manager i databasen 

        }
        // adds a new employee to a department
        public void AddNewDepartmentEmployee(int departmentId, Employee newEmployee)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            SqlCommand command = new SqlCommand(
                "INSERT INTP EmployeeDepartment (E_ID, D_ID) VALUES (@e,@d)",
                 connection);

            command.Parameters.AddWithValue("@e", newEmployee.Id);
            command.Parameters.AddWithValue("@d", departmentId);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        // removes stock from a department's warehouse
        public void RemoveStock(Ingredient ingredient, int amount, int departmentID, int warehouseID)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            connection.Open();

            SqlCommand update = new SqlCommand(
                "UPDATE Ingredient SET I_quntity = I_quntity - @amount WHERE I_ID = @id",
                connection);

            update.Parameters.AddWithValue("@amount", amount);
            update.Parameters.AddWithValue("@id", ingredient.ID);
            update.ExecuteNonQuery();

            SqlCommand getQuantity = new SqlCommand(
                "SELECT I_quntity FROM Ingredient WHERE I_ID = @id",
                connection);

            getQuantity.Parameters.AddWithValue("@id", ingredient.ID);

            object quantityObj = getQuantity.ExecuteScalar();

            if (quantityObj == null || quantityObj == DBNull.Value)
            {
                connection.Close();
                return;
            }

            // convert database value to int
            int quantity = Convert.ToInt32(quantityObj);

            if (quantity <= 0)
            {
                SqlCommand removeLink = new SqlCommand(
                    "DELETE FROM werehouseIngredient WHERE W_ID = @w AND I_ID = @i",
                    connection);

                removeLink.Parameters.AddWithValue("@w", warehouseID);
                removeLink.Parameters.AddWithValue("@i", ingredient.ID);
                removeLink.ExecuteNonQuery();
            }

            connection.Close();


        }
    }
}
