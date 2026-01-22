<h1 align="center">HexTicketing - HexTS</h1>

<p align="center">
A streamlined, full stack support ticketing platform built with <strong>ASP.NET Core</strong> and <strong>Angular</strong>.  
Designed for organizations that require clarity, accountability and a structured workflow for managing internal support operations.
</p>

<p align="center">
  <img src="images/Wellcome.png" width="750">
</p>

---

## Overview

HexTicketing delivers a unified environment for reporting issues, assigning responsibilities and ensuring smooth collaboration between users, support agents and administrators.  

The platform opens with an animated welcome screen and transitions smoothly into a role-based interface tailored to each user type. Every interaction from ticket creation to final resolution is designed to remain intuitive, organized and predictable.

---

## Administrative Ecosystem

The **Admin Dashboard** acts as the operational command center. It provides complete visibility into ticket distribution across states such as New, Assigned, In Progress and Resolved.

<p align="center">
  <img src="images/AdminDashboard.png" width="750">
</p>

### User Management

HexTicketing includes a powerful user lifecycle and permission management module:

**Role Progression**  
New registrations begin as **Users**. Administrators elevate users to **Support Agent** or **Admin** roles based on organizational needs.

**Directory Controls**  
Admins can search users, activate or deactivate accounts and perform permanent deletions when necessary.

**Instant Role Assignment**  
User roles can be reassigned immediately via the directory interface.

<p align="center">
  <img src="images/AdminUserDirectory.png" width="750">
</p>

---

## Ticket Orchestration

Administrators maintain complete oversight into ticket flow and ensure service quality standards.

**Assignment**  
Unassigned tickets can be assigned directly to Support Agents based on priority or specialization.

**Collaboration**  
Administrators can open any ticket, view its conversation thread and contribute guidance or clarification when required.

<p align="center">
  <img src="images/Ticket.png" width="750">
</p>

---

## Support Agent Workspace

The Support Agent view is optimized for clarity and efficient task execution, ensuring agents focus solely on assigned responsibilities.

<p align="center">
  <img src="images/SupportAgentDashboard.png" width="750">
</p>

### Agent Capabilities

**My Workspace**  
A dedicated panel showing only tickets assigned to the current agent.

**Status Management**  
Agents can move tickets through logically enforced stages:  
• New  
• In Progress  
• Resolved  

**Contextual Understanding**  
Agents have complete access to ticket metadata and conversation history, enabling informed interactions and quicker resolutions.

---

## Client Portal

The client facing interface emphasizes simplicity. Customers can create tickets, add details and track their status in real time.

<p align="center">
  <img src="images/UserDashboard.png" width="750">
</p>

### Ticket Lifecycle

**Creation**  
Users initiate tickets by entering a title, description and selecting an intensity level.

**Priority Indicators**  
HexTicketing uses a clear visual priority scheme:

🔴 **High** 🟡 **Medium** 🔵 **Low** 

**Tracking**  
Tickets are automatically grouped into  
• Unassigned  
• In Progress  
• Resolved  

**Self-Resolution**  
Users may close their own tickets after the issue is resolved satisfactorily.

<p align="center">
  <img src="images/UserCreateTicket.png" width="750">
</p>

---

## Access & Security

HexTicketing employs a secure authentication layer based on JSON Web Tokens (JWT).  
Sensitive configuration values remain excluded from the repository via `.gitignore`.

<p align="center">
  <img src="images/Register.png" width="350">
  <img src="images/Login.png" width="350">
</p>

### Key Technical Features

• Role based access control  
• Dynamic assignment and promotion of user roles  
• Modern Angular UI with smooth transitions  
• Well-structured API endpoints aligned with REST principles  
• Logical, enforced ticket state transitions to maintain workflow integrity  

---

## Technology Stack

**Frontend**  
Angular, TypeScript, HTML/CSS  

**Backend**  
ASP.NET Core Web API (C#)  

**Database**  
SQL Server (SSMS)  

**Authentication**  
JWT-based authorization  

---

<h2 align="center">HexTS | Effortless Support Ticketing System</h2> <p align="center">A modern approach to transparent, organized and efficient technical support ticketing system.</p>
