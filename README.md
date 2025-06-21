# CaseGuard Infra – AWS CDK Deployment

This project is part of the CaseGuard DevOps Assessment. The objective is to provision and deploy a scalable, secure, and reusable cloud infrastructure using AWS CDK in C#.

---

## 🚀 Project Overview

We designed an infrastructure-as-code solution using AWS CDK (in C#) to deploy a secure, containerized API (`ProductManagementSystem`) into two AWS regions. We built out staging and production environments, with CI/CD automation through GitHub Actions.

---

## 📦 Architecture Summary

### Regional Stacks
- **USEast1 (Staging)** – Deploys `NetworkStack`, `ComputeStack`, `DatabaseStack`.
- **USWest2 (Production)** – Same architecture with regional separation.

### Stacks
- **NetworkStack**: Creates a VPC with public subnets.
- **ComputeStack**: ECS Fargate service behind an Application Load Balancer.
- **DatabaseStack**: PostgreSQL RDS instance with secrets management.

---

## 🛠️ Prerequisites

| Tool           | Version      |
|----------------|--------------|
| AWS CDK        | 2.x          |
| Node.js        | >=20.x       |
| .NET SDK       | 8.0+         |
| AWS CLI        | Configured   |

### GitHub Secrets

- `AWS_ACCESS_KEY_ID`
- `AWS_SECRET_ACCESS_KEY`
- `AWS_REGION`

---

## 📂 Project Structure

```
caseguard-infra/
├── .github/workflows/
│   └── deploy.yml
├── src/CaseGuardInfra/
│   ├── App.cs
│   ├── NetworkStack.cs
│   ├── ComputeStack.cs
│   └── DatabaseStack.cs
└── README.md
```

---

## 🔁 CI/CD Pipeline

GitHub Actions workflow deploys automatically on push to `main`.

- Deploys to **staging (us-east-1)** immediately.
- Requires **manual approval** for production (us-west-2).

### Sample Workflow Logic

```yaml
jobs:
  deploy:
    steps:
      - run: dotnet restore
      - run: cdk deploy Staging* --require-approval never
  manual-approval:
    environment: production
    steps:
      - run: cdk deploy Prod* --require-approval never
```

---

## 🚀 Deployment Instructions

```bash
# Clone repo
git clone https://github.com/MadhuVivekBorra/caseguard-infra.git
cd caseguard-infra

# Install tools
npm install -g aws-cdk
dotnet tool install -g Amazon.CDK.GlobalTool

# Restore and bootstrap
dotnet restore src/CaseGuardInfra/CaseGuardInfra.csproj
cdk bootstrap aws://<account>/<region>

# Deploy manually
cdk deploy --all --require-approval never
```

---

## 🧠 Architecture Design Decisions

- **Reusable Stacks**: Modular stack structure for network, compute, and DB.
- **Security**: RDS secrets managed via Secrets Manager, public subnets used for simplicity.
- **Separation**: Two regions simulate staging vs production deployment pipelines.

---

## ⚠️ Known Limitations

- Node.js 18.x reached EOL (upgrade needed to Node 20+).
- Master username "admin" caused a failure – changed to supported value.
- Only simple logging; no enhanced observability (e.g., CloudWatch dashboards).
- No custom domain/SSL for ALB endpoints.
- Environment config is static – can improve with parameter store or config files.

---

## 📈 Possible Enhancements

- Add CloudWatch dashboards and alarms.
- Integrate SSM Parameter Store for dynamic config.
- Add custom domain name via Route53 + ACM.
- Use private subnets for DB and ECS tasks (plus NAT Gateway).
- Use Terraform Cloud or AWS CodePipeline for production-grade automation.

---

## ✅ Status Summary

| Feature                          | Status  |
|----------------------------------|---------|
| CDK Infra (multi-region)         | ✅ Done |
| ECS Fargate Service + ALB        | ✅ Done |
| RDS + Secrets                    | ✅ Done |
| CI/CD (GitHub Actions)           | ✅ Done |
| Manual Approval for Prod         | ✅ Done |
| Documentation                    | ✅ Done |
| Architecture Diagram (Pending)  | ❌ Optional |

---

## 📚 References

- [AWS CDK Docs](https://docs.aws.amazon.com/cdk/latest/guide/home.html)
- [GitHub Actions](https://docs.github.com/en/actions)
- [CDK Patterns](https://cdkpatterns.com/)

---

**Author:** Madhu Vivek Borra  
**Repo:** https://github.com/MadhuVivekBorra/caseguard-infra.git
