# MinIOFileProcessor - System A

This project handles the upload of CSV files to [MinIO](https://github.com/minio/minio) — a local simulation of Amazon S3. After uploading, it stores the file's storage path in a MySQL database and publishes a message to a Kafka topic to notify downstream services.

---

## 🚀 Motivation

In a real-world scenario, I encountered the need to upload CSV files into an internal system. These files could be quite large (up to 7MB with ~100,000 rows), making real-time validation inefficient and performance-heavy.

To address this, a more scalable and efficient approach was adopted:

1. ✅ Validate the MIME type to ensure it's a genuine CSV file  
2. ✅ Validate the CSV header for required structure  
3. ✅ Ensure the file size does not exceed 7MB  

After this initial validation and upload, a background service ([System B](https://github.com/JGMelon22/MinIOFileConsumer)) will:

- Consume messages from the Kafka topic
- Check whether the file has already been processed (based on the `Status` column in the database)
- Process the file's content according to business rules asynchronously

---

## 🗺️ Project Structure
![diagram](https://github.com/user-attachments/assets/a531f1f8-af8a-49fe-8c6d-4f9cfef49f23)

---

## 🧰 Tech Stack

<div style="display: flex; gap: 10px;">
    <img height="32" width="32" src="https://cdn.simpleicons.org/dotnet" alt=".NET" title=".NET" />
    <img height="32" width="32" src="https://cdn.simpleicons.org/swagger" alt="Swagger" title="Swagger" />
    <img height="32" width="32" src="https://cdn.simpleicons.org/mysql" alt="MySQL" title="MySQL" />
    <img height="32" width="32" src="https://cdn.simpleicons.org/minio" alt="MinIO" title="MinIO" />
    <img height="32" width="32" src="https://cdn.simpleicons.org/apachekafka" alt="Apache Kafka" title="Apache Kafka" />
</div>

<br/>

- **.NET** – Main backend framework  
- **Swagger** – API documentation  
- **MySQL** – Relational database to store metadata  
- **MinIO** – Local S3-compatible object storage  
- **Apache Kafka** – Event streaming and message queuing

---

## 🙏 Acknowledgments

- Project structure diagram created using [GitDiagram](https://gitdiagram.com/) by [@ahmedkhaleel2004](https://github.com/ahmedkhaleel2004)
