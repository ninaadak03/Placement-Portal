using Backend.Entities;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class TestDataSeeder
{
    private readonly ApplicationDbContext _context;

    public TestDataSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedStudentsAsync()
    {
        // if (await _context.Students.AnyAsync())
        // {
        //     Console.WriteLine("Students already exist. Skipping test data seeding.");
        //     return;
        // }

        var excelPath = Path.Combine(AppContext.BaseDirectory,"Data","PlacementPortal_Students.xlsx");

        if (!File.Exists(excelPath))
        {
            throw new FileNotFoundException($"Could not find Excel file at: {excelPath}");
        }

        using var workbook = new XLWorkbook(excelPath);

        var worksheet = workbook.Worksheet("Students");

        using var transaction = await _context.Database.BeginTransactionAsync();

        int currentRowNumber = 1;
        string currentRollNo = string.Empty;

        try
        {
            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                currentRowNumber = row.RowNumber();
                currentRollNo = row.Cell(1).GetString().Trim();
                
                string rollNo = currentRollNo;
                if (await _context.Users.AnyAsync(u => u.RollNo == rollNo))
                {
                    Console.WriteLine($"Skipping existing Roll Number: {rollNo}");
                    continue;
                }

                string name = row.Cell(2).GetString().Trim();
                string email = row.Cell(3).GetString().Trim();
                string phoneNumber = row.Cell(4).GetString().Trim();
                string branch = row.Cell(5).GetString().Trim();
                string gender = row.Cell(6).GetString().Trim();

                DateOnly dateOfBirth = DateOnly.Parse(row.Cell(7).GetString());;

                decimal tenthPercentage = row.Cell(8).GetValue<decimal>();

                decimal twelfthPercentage = row.Cell(9).GetValue<decimal>();

                decimal sgpa1 = row.Cell(10).GetValue<decimal>();
                decimal sgpa2 = row.Cell(11).GetValue<decimal>();
                decimal sgpa3 = row.Cell(12).GetValue<decimal>();
                decimal sgpa4 = row.Cell(13).GetValue<decimal>();
                decimal sgpa5 = row.Cell(14).GetValue<decimal>();
                decimal sgpa6 = row.Cell(15).GetValue<decimal>();

                var user = new User
                {
                    Email = email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                    Role = "Student",
                    RollNo = rollNo,
                    IsVerified = true
                };

                _context.Users.Add(user);

                await _context.SaveChangesAsync();

                decimal cgpa = CalculateCgpa(sgpa1, sgpa2, sgpa3, sgpa4, sgpa5, sgpa6);

                var student = CreateStudent(
                    user.Id,
                    rollNo,
                    name,
                    phoneNumber,
                    branch,
                    gender,
                    dateOfBirth,
                    tenthPercentage,
                    twelfthPercentage,
                    sgpa1,
                    sgpa2,
                    sgpa3,
                    sgpa4,
                    sgpa5,
                    sgpa6,
                    cgpa);

                _context.Students.Add(student);

                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();

            Console.WriteLine("Student test data imported successfully.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            throw new Exception(
                $"Student import failed.\n\n" +
                $"Excel Row : {currentRowNumber}\n" +
                $"Roll Number : {currentRollNo}\n\n" +
                $"Reason : {ex.Message}", ex);
        }
    }

    private Student CreateStudent(
        int userId,
        string rollNo,
        string name,
        string phoneNumber,
        string branch,
        string gender,
        DateOnly dateOfBirth,
        decimal tenthPercentage,
        decimal twelfthPercentage,
        decimal sgpa1,
        decimal sgpa2,
        decimal sgpa3,
        decimal sgpa4,
        decimal sgpa5,
        decimal sgpa6,
        decimal cgpa)
    {
        return new Student
        {
            UserId = userId,

            Name = name,
            RollNo = rollNo,
            PhoneNumber = phoneNumber,
            Branch = branch,
            Gender = gender,
            DateOfBirth = dateOfBirth,

            TenthPercentage = tenthPercentage,
            TwelfthPercentage = twelfthPercentage,

            SgpaSem1 = sgpa1,
            SgpaSem2 = sgpa2,
            SgpaSem3 = sgpa3,
            SgpaSem4 = sgpa4,
            SgpaSem5 = sgpa5,
            SgpaSem6 = sgpa6,

            // Final year not completed
            SgpaSem7 = null,
            SgpaSem8 = null,

            CGPA = cgpa,

            ResumeUrl = GenerateResumeUrl(rollNo),

            IsPlaced = false,
            IsProfileCompleted = true,

            PlacedCompanyId = null,
            PlacedCTC = null
        };
    }

    private static decimal CalculateCgpa(
        decimal sgpa1,
        decimal sgpa2,
        decimal sgpa3,
        decimal sgpa4,
        decimal sgpa5,
        decimal sgpa6)
    {
        decimal average = (sgpa1+sgpa2+sgpa3+sgpa4+sgpa5+sgpa6)/6m;
        return Math.Round(average, 2, MidpointRounding.AwayFromZero);
    }

    private static string GenerateResumeUrl(string rollNo)
    {
        return $"https://example.com/resumes/{rollNo}.pdf";
    }
}