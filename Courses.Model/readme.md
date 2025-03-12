# Миграции для бд курсов

1) Для создания первой миграции комментируем строки с `Database.EnsureCreated`. После чего выполняем следующую команду `dotnet ef migrations add InitialCreate --project ./CourseBackend/Courses.Model --startup-project ./Api/Courses.AdminPanel.csproj --output-dir AdminPanelMigrations`. Выполнять команду следует из корня репозитория проекта, использующего `CourseBackend.Courses.Model` как подмодуль.

2) Для применения миграции и создания бд на её основе выполняем команду `dotnet ef database update --project ./CourseBackend/Courses.Model --startup-project ./Api/Courses.AdminPanel.csproj`. Так как сборки во всех средах кроме `Development` используют переменные окружения, то должны быть заполнены `DB_USER`, `DB_PASSWORD`, `DB_NAME` и `DB_PORT`.