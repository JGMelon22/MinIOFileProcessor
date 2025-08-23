# MinIOFileProcessor - System A (Observability Branch)

This project handles the upload of CSV files to MinIO — a local simulation of Amazon S3. After uploading, it stores the file's storage path in a MySQL database and publishes a message to a Kafka topic to notify downstream services.

---

## 🔎 Extension with Observability

This branch extends the original project by **including observability tools** to improve monitoring, troubleshooting, and performance insights.  

The extensions include:  
- **Health Checks** – Liveness and readiness endpoints.  
- **Custom Kafka Metrics** – Exported via [kafka_exporter](https://github.com/danielqsj/kafka_exporter).  
- **Prometheus** – Metrics collection and alerting.  
- **Grafana** – Dashboards for visualization and analysis.  
- **Jaeger** – Distributed tracing with OpenTelemetry.  
- **Serilog + SEQ** – Structured logging for centralized log search.  

With these tools, the system not only processes file uploads and notifications, but also provides full visibility into its health, metrics, and traces.

---

## 🚀 Motivation

In real-world scenarios, large CSV file uploads (up to 7MB and ~100,000 rows) can make real-time validation inefficient.  
To improve scalability and maintainability, the system adopts a two-phase approach:

1. ✅ Validate MIME type, header, and file size (≤ 7MB).  
2. ✅ Upload to MinIO and store metadata in MySQL.  
3. ✅ Publish a message to Kafka to notify downstream services.  

A background service ([System B](https://github.com/JGMelon22/MinIOFileConsumer)) then asynchronously processes the file content based on business rules.

---

## 🗺️ Project Architecture
![diagram](https://github.com/user-attachments/assets/a531f1f8-af8a-49fe-8c6d-4f9cfef49f23)

---

## 🧰 Tech Stack

<div style="display: flex; gap: 10px;">
    <img height="32" width="32" src="https://cdn.simpleicons.org/dotnet" alt=".NET" title=".NET" />
    <img height="32" width="32" src="https://cdn.simpleicons.org/swagger" alt="Swagger" title="Swagger" />
    <img height="32" width="32" src="https://cdn.simpleicons.org/mysql" alt="MySQL" title="MySQL" />
    <img height="32" width="32" src="https://cdn.simpleicons.org/minio" alt="MinIO" title="MinIO" />
    <img height="32" width="32" src="https://cdn.simpleicons.org/apachekafka" alt="Apache Kafka" title="Apache Kafka" />
    <img height="32" width="32" src="https://cdn.simpleicons.org/jaeger" alt="Jaeger" title="Jaeger" />
    <img height="32" width="32" src="https://cdn.simpleicons.org/prometheus" alt="Prometheus" title="Prometheus" />
    <img height="32" width="32" src="https://cdn.simpleicons.org/grafana" alt="Grafana" title="Grafana" />
    <img height="32" width="32" src="https://cdn.simpleicons.org/opentelemetry" alt="OpenTelemetry" title="OpenTelemetry" />
</div>

<br/>

- **.NET** – Backend framework.  
- **Swagger** – API documentation.  
- **MySQL** – Relational database for metadata.  
- **MinIO** – S3-compatible object storage (local).  
- **Apache Kafka** – Event streaming and queuing.  
- **Jaeger** – Distributed tracing platform.  
- **Grafana** – Visualization and dashboards.  
- **Prometheus** – Monitoring & alerting system.  
- **Serilog** – Structured logging for .NET.  
- **SEQ** – Real-time log search and analysis.  

---

## 📊 Observability in Action

### 🔧 Custom Dashboards
Three Grafana dashboards are included in the project under the [`dashboards/`](./dashboards) directory:  
- **ASP.NET Runtime and Processes Dashboard**
- **HTTP Metrics Dashboard**  
- **Simple Kafka Producer Dashboard**  

These can be imported directly into Grafana to monitor application runtime, Kafka producers, and HTTP-level metrics.

### 🖼️ Example Visualizations

- Example **Grafana dashboards** with application and Kafka metrics.  
<img width="800" alt="Captura de tela 2025-08-23 114902" src="https://github.com/user-attachments/assets/88ab1bed-8272-42fb-aceb-65cdf90f4d59" />
<img width="800" alt="Captura de tela 2025-08-23 115219" src="https://github.com/user-attachments/assets/8fa282d1-eecf-41ee-a752-9a56576748d0" />
<img width="800" alt="Captura de tela 2025-08-23 115303" src="https://github.com/user-attachments/assets/d985f5d3-23fb-494b-a707-176745fa0d9f" />

- Sample **Jaeger trace** showing request flow across services.  
<img width="800" alt="Captura de tela 2025-08-23 115336" src="https://github.com/user-attachments/assets/faa3c9b8-3f79-4f1c-889d-f99f29ceb259" />

- **SEQ dashboard** for structured logging and analysis.  
<img width="800" alt="Captura de tela 2025-08-23 120005" src="https://github.com/user-attachments/assets/d47575ef-a001-4438-8cd6-ee1616c5ebb4" />

- Health check endpoints exposed under `/health`.  
<img width="800" alt="Captura de tela 2025-08-23 115452" src="https://github.com/user-attachments/assets/b6a11210-6bc8-4281-9f57-997fde785e6e" />

---

## 🙏 Acknowledgments

- **Kafka metrics** powered by [kafka_exporter](https://github.com/danielqsj/kafka_exporter) by [@danielqsj](https://github.com/danielqsj)  
- **Architecture diagram** generated with [GitDiagram](https://gitdiagram.com/) by [@ahmedkhaleel2004](https://github.com/ahmedkhaleel2004)  
