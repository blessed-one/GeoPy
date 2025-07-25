
# Oil Wells App - система учета и управления скважинами | GeoPy


[![Docker](https://img.shields.io/badge/Docker-✓-blue)](https://www.docker.com)
[![Docker Compose](https://img.shields.io/badge/Docker--Compose-Used-2496ed?logo=docker&logoColor=white)](https://docs.docker.com/compose/)
[![.NET](https://img.shields.io/badge/.NET-9-purple)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/Database-PostgreSQL-336791?logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Vite](https://img.shields.io/badge/Frontend-Vite-646CFF?logo=vite&logoColor=white)](https://vitejs.dev/)
[![React](https://img.shields.io/badge/Library-React-61dafb?logo=react&logoColor=white)](https://reactjs.org/)

## Как запустить
1. Установить [Docker](https://www.docker.com/)
2. Скачать проект
```bash
git clone https://github.com/blessed-one/GeoPy.git
cd GeoPy
```
3. Сборка и запуск проекта в Docker через терминал
```bash
docker-compose up -d --build
```

**После запуска:**
-   API: [http://localhost:5001/swagger](http://localhost:5001/swagger)
-   Frontend: [http://localhost:5173](http://localhost:5173)

## API Endpoints

### Аутентификация (JWT Bearer)
| Метод | Endpoint          | Описание                |
|-------|-------------------|-------------------------|
| POST  | /api/auth/login   | Вход в систему          |
| POST  | /api/auth/register| Регистрация пользователя|

### Импорт/Экспорт Excel файла

| Метод | Endpoint			  | Описание                |
|-------|---------------------|-------------------------|
| POST  | /api/wells/import   | Импорт данных из Excel в бд|
| GET   | /api/wells/export   | Экспорт из бд в Excel         |


### Скважины
| Метод | Endpoint                | Описание                |
|-------|-------------------------|-------------------------|
| GET   | /api/wells              | Все скважины            |
| GET   | /api/wells/{id}         | Скважина по ID          |
| POST  | /api/wells              | Создать скважину        |
| PUT   | /api/wells/{id}         | Обновить скважину       |
| DELETE| /api/wells/{id}         | Удалить скважину        |
