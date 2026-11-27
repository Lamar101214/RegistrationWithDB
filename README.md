# Club Registration — IT1811 Laboratory Exercise 7

## Open and run
1. Extract the entire ZIP into a writable folder.
2. In Visual Studio Installer, install **.NET desktop development**, the **.NET Framework 4.8 targeting pack**, and **SQL Server Express LocalDB**. Visual Studio 2019 or later is recommended for this project. No NuGet packages are needed.
3. Open `RegistrationWithDB.sln`, then press F5.
4. On first run the app creates ClubDB and ClubMembers on `(localdb)\MSSQLLocalDB`. If ClubDB already exists, it is reused and no existing table is overwritten. Its table must have the columns in the handout.
5. Register a student, use Refresh to reload the list, and use Update to select a Student ID, edit its details, and Confirm. The main grid refreshes after a successful update.

## Windows Forms Designer
Both forms include `.cs`, `.Designer.cs`, and `.resx` files. Right-click **FrmClubRegistration.cs** or **FrmUpdateMember.cs** in Solution Explorer and select **View Designer** (Shift+F7). Change control positions, fonts, colors, and sizes in the Properties panel. All controls are declared and initialized in InitializeComponent; constructors only call InitializeComponent. Database operations run in events, not in the designer.

## Database / service-based database requirement
A binary MDF is not bundled: SQL Server LocalDB must create it on Windows. The automatic first-run setup creates actual ClubDB.mdf and ClubDB_log.ldf files in `%LOCALAPPDATA%\RegistrationWithDB` when it creates a new database. This stable folder prevents rebuilds from replacing saved data.

For the handout's manual **Add > New Item > Service-based Database** step:
1. Before the first run, create `ClubDB.mdf` in the project using that menu.
2. Open it in Server Explorer and run the CREATE TABLE statement from ClubDB.sql against that database (omit USE/CREATE DATABASE sections).
3. Replace DatabaseSetup.ConnectionString's getter with your database's connection string copied from Properties. Use a stable absolute AttachDbFilename path. Set the MDF Copy to Output Directory to **Do not copy** to prevent separate/reset databases.
4. Remove only `DatabaseSetup.Initialize();` from FrmClubRegistration.ConnectAndRefresh because you created the database manually. The query class will use your connection string.

Alternatively, keep automatic setup and inspect the resulting database via SQL Server Object Explorer > Add SQL Server > `(localdb)\MSSQLLocalDB` > Databases > ClubDB. `ClubDB.sql` provides a standalone manual setup option as well.

## Included requirements
- Two editable forms matching the sample's arrangement.
- ClubRegistrationQuery with the named fields and DisplayList, RegisterStudent methods.
- Parameterized SELECT, INSERT, and UPDATE using System.Data.SqlClient.
- RegistrationID uses count and retrieves the current maximum ID before incrementing, so restarting does not reset the counter. Intended for a single-user laboratory application; simultaneous inserts with the same ID are rejected, not overwritten.
- Student ID selection loads all existing fields before updating.
- Input checks, optional middle name, duplicate-ID handling, and safe connection disposal.
- The handout's incorrect @RegistrationID parameter and VARCHAR StudentID parameter are corrected to a single @StudentID of SqlDbType.BigInt.
- The seven displayed columns exclude internal ID, as requested.

## Manual verification on Windows
1. Register Student ID 20260001, first name Ana, middle name Cruz, last name Santos, age 20, Female, BS Information Technology. Check that one row appears.
2. Try the same Student ID again: it must be rejected without adding a row.
3. Try blank names, an invalid/overflowing Student ID, missing program, and age 0: each must be rejected.
4. Open Update: no member is selected and Confirm is disabled. Select 20260001 and confirm all fields populate.
5. Change age to 21, Confirm, then Yes: the main grid must show 21. Choose No to verify an update is not saved.
6. Close and reopen: the row remains. Add a different Student ID and verify registration succeeds.
7. On an empty database, open Update and verify the no-members message.
8. Open both forms in Designer and verify controls can be selected and moved.

Validation in the creation environment: project XML, resources, source references, and event wiring were checked. Compilation, Windows Forms Designer rendering, and SQL execution were not run because this Linux environment has no .NET/Mono compiler or SQL Server LocalDB. Run the checklist above on Windows before submission.

The folder uses Talabucon_Laurenz_RegistrationWithDB; rename it if your required submission name differs.
