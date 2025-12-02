1. Project Overview

This repository contains the final submission for the CO3011 Software Project Management module.

The project is a full-stack, containerized, multi-language web application designed to support university library operations, including:

registration

authentication

member management

book management

borrowing and returning

reporting

Development followed the Scrum framework over 5 sprints, with rotating roles (PO, SM, BA, Dev), backlog refinement, requirement injections, sprint reviews, and retrospectives.

2. Key Features (MVP Scope)
2.1 Authentication & User Management

Student / staff registration

Login and session management

JWT-based authentication

Password confirmation & validation

Form validation test suite (40+ automated cases)

2.2 Library Operations

Book search

Book management (add, update, delete)

Borrow and return workflow

Overdue detection

Member point system

2.3 Reporting & Logs

Borrow history

Overdue report

Activity logs stored in MongoDB Atlas

2.4 Development Support

Automated test cases (UI + API)

Dockerized services

Multiple sprint increments

Low-fi prototype in Figma

System design diagrams (Sprint 1)

3. System Design Diagrams

System Design (Sprint 1)
https://drive.google.com/file/d/1KzWkvfCnfZVO2V8e6m8LBMh1p5LihTbV/view?usp=sharing

Figma Low-Fi Prototype (Sprint 2)
https://www.figma.com/design/XMyyLNbycULmpWsaJKQW0Q/CO3011?node-id=0-1

4. Tech Stack
Category	Technology	Notes
Front-end	React (TypeScript)	Vite for tooling & build optimization (Sprint 3)
Back-end	.NET Core 9.0	API development framework (Sprint 4)
Authentication	FastAPI	Auth microservice (Sprint 2)
Database (NoSQL):	MongoDB Atlas	Cloud NoSQL (Sprint 5)
Database (SQL):	PostgreSQL	Relational storage (Sprint 5)
Deployment	AWS + Docker	Docker setup completed (Sprint 4)
Testing Strategy
Category	Tools	Status
API Testing	Postman	Automated testing (Sprint 4)
UI/E2E Testing	Selenium	Automated testing (Sprint 5)
5. Manual Execution
5.1 Backend (.NET Core)
cd backend
dotnet restore
dotnet run

5.2 Authentication Service (FastAPI)
cd auth
pip install -r requirements.txt
uvicorn main:app --reload

5.3 Frontend (React + Vite)
cd frontend
npm install
npm run dev

6. Environment Variables
6.1 FastAPI (.env)
JWT_SECRET=your_secret
POSTGRES_URL=postgresql://...

6.2 .NET Core API (.env)
POSTGRES_CONNECTION_STRING=...
MONGO_URI=...

6.3 Frontend (.env)
VITE_API_URL=http://localhost:5173
VITE_AUTH_URL=http://localhost:8000

7. API Endpoints (Summary)
7.1 Authentication Service (FastAPI)

POST /auth/login

POST /auth/register

POST /auth/reset

7.2 Core Service (.NET Core 9)

GET /books

POST /books

PUT /books/{id}

POST /borrow

POST /return

GET /members/{id}

GET /reports/overdue

8. Repository Structure
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

10. Contributors (Group CC01-01)

Nguyễn Háo Hồng Dũ

Phan Phước Hưng

Nguyễn Tiến Khang

Trương Minh Khang

Trần Nguyễn Anh Khoa

Nguyễn Thành Phát

Nguyễn Minh Quân

Roles rotated across: Product Owner, Scrum Master, Business Analyst, Developer.

11. Notes for Instructors

This repository includes all sprint deliverables, including prototypes, diagrams, and automated test cases.

The project follows Scrum principles with incremental delivery.

CI/CD pipeline setup is optional for the scope of this module.
