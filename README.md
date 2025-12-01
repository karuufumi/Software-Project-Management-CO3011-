1. Project Overview

This repository contains the final submission for the CO3011 Software Project Management module.
The project is a full-stack, containerized, multi-language web application designed to support university library operations, including registration, authentication, member management, book management, borrowing, returning, and reporting. Development followed the Scrum framework over 5 sprints, with rotating roles (PO, SM, BA, Dev), backlog refinement, requirement injections, sprint reviews, and retrospectives.

2. Key Features (MVP Scope)
2.1. Authentication & User Management:
- Student / staff registration
- Login and session management
- JWT-based authentication
- Password confirmation & validation
- Form validation test suite (40+ automated cases)
2.2. Library Operations
- Book search
- Book management (add, update, delete)
- Borrow and return workflow
- Overdue detection
- Member point system
2.3. Reporting & Logs
- Borrow history
- Overdue report
- Activity logs stored in NoSQL (MongoDB Atlas)
2.4. Development Support
- Automated test cases (UI + API)
- Dockerized services
- Multiple sprint increments
- Low-fi prototype in Figma
- System design diagrams (Sprint 1)

3. System Design Diagrams
https://drive.google.com/file/d/1KzWkvfCnfZVO2V8e6m8LBMh1p5LihTbV/view?usp=sharing
(implemented in Sprint 1)
Figma (Low-Fi) Prototype :
https://www.figma.com/design/XMyyLNbycULmpWsaJKQW0Q/CO3011?node-id=0-1&t=c9WUnjP9DfN6TMNa-1
(implemented in Sprint 2)


4.  Tech Stack
| Category | Technology | Notes |
| :--- | :--- | :--- |
| **Front-end** | **React** (Typescript) | Utilizes **Vite** for tooling and build optimization. **implemented in Sprint 3** |
| **Back-end** | **.NET Core 9.0** | Core API development framework. **to be implemented in Sprint 4** |
| **Authentication** | **FastAPI** | Used specifically for managing authentication services. **implemented in Sprint 2** |
| **Database (NoSQL)** | **Atlas MongoDB** | Using Atlas cloud solution. **to be implemented in Sprint 5** |
| **Database (SQL)** | **PostgreSQL** | Relational data storage. **to be implemented in Sprint 5** |
| **Deployment** | **AWS** + **Docker** | Deployment target is **AWS**. **Docker** containerization is scheduled (implemented in Sprint 4) |


## Testing Strategy

| Category | Tools | Status |
| :--- | :--- | :--- |
| **API Testing** | **Postman** | Automated testing **to be implemented in Sprint 4**. |
| **UI/E2E Testing** | **Selenium** | Automated testing **to be implemented in Sprint 5**. |

5. Manual Execution
5.1. Backend (.NET):
  cd backend
  dotnet restore
  dotnet run
5.2. Authentication Service (FastAPI)
   cd auth
  pip install -r requirements.txt
  uvicorn main:app --reload
5.3. Frontend (React + Vite)
   cd frontend
    npm install
    npm run dev
6. Environment Variables
   6.1. FastAPI(.env):
     JWT_SECRET=your_secret
     POSTGRES_URL=postgresql://...
    6.2. .NET Core API (.env)
     POSTGRES_CONNECTION_STRING=...
     MONGO_URI=...
   6.3. Frontend (.env)
     VITE_API_URL=http://localhost:5173
     VITE_AUTH_URL=http://localhost:8000
   7. API Endpoints (Summary)
Authentication Service (FastAPI)
- POST /auth/login
- POST /auth/register
- POST /auth/reset

Core Service (.NET Core 9)
- GET /books
- POST /books
- PUT /books/{id}
- POST /borrow
- POST /return
- GET /members/{id}
- GET /reports/overdue
8. Repository structure
  Software-Project-Management-CO3011-
│
├── frontend/             
├── backend/               
├── auth/                  
├── tests/                  
├── diagrams/              
├── figma/                
├── docker-compose.yml
└── README.md
9. Contributors (Group CC01-01)

- Nguyễn Háo Hồng Dũ
- Phan Phước Hưng
- Nguyễn Tiến Khang
- Trương Minh Khang
- Trần Nguyễn Anh Khoa
- Nguyễn Thành Phát
- Nguyễn Minh Quân
Roles rotated across: Product Owner, Scrum Master, Business Analyst, Developer.

10.  Notes for Instructors

- This repository includes all sprint deliverables, including test plans, prototypes, and automated test cases.
- The project reflects Scrum methodology with clear increments per sprint.
- CI/CD pipeline setup is optional for this submission.



---
