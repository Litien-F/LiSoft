# 🚀 Setup - LiSoft

## ⚠️ IMPORTANTE - Configuração Inicial

### 1. Configurar MongoDB Connection String

O arquivo `appsettings.json` **NÃO** está versionado por segurança. Você precisa criar/configurar:

#### Opção A: Copiar do exemplo
```bash
cd backend/LiSoft.Api
copy appsettings.example.json appsettings.json
```

#### Opção B: Criar manualmente
Crie o arquivo `backend/LiSoft.Api/appsettings.json` com:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "MongoDbSettings": {
    "ConnectionString": "SUA_CONNECTION_STRING_AQUI",
    "DatabaseName": "system_lisoft",
    "Collections": {
      "Contacts": "contacts"
    }
  }
}
```

**Substitua** `SUA_CONNECTION_STRING_AQUI` pela sua connection string do MongoDB Atlas.

### 2. Instalar Dependências

#### Backend (.NET)
```bash
cd backend/LiSoft.Api
dotnet restore
dotnet build
```

#### Frontend (React)
```bash
cd frontend
npm install
```

### 3. Executar a Aplicação

#### Terminal 1 - Backend
```bash
cd backend/LiSoft.Api
dotnet run
```
API disponível em: `http://localhost:5000`

#### Terminal 2 - Frontend
```bash
cd frontend
npm start
```
Frontend disponível em: `http://localhost:3000`

## 🔐 Segurança

### Arquivos que NÃO devem ser commitados:
- ❌ `appsettings.json` (contém credenciais)
- ❌ `appsettings.Development.json` (pode conter credenciais)
- ❌ `node_modules/` (dependências do npm)
- ❌ `bin/` e `obj/` (arquivos compilados)

### Arquivos que DEVEM ser commitados:
- ✅ `appsettings.example.json` (template sem credenciais)
- ✅ `.gitignore` (configurado corretamente)
- ✅ Código fonte

## 📝 Variáveis de Ambiente (Produção)

Para produção, use variáveis de ambiente ao invés de `appsettings.json`:

```bash
# Windows
set MongoDbSettings__ConnectionString=mongodb+srv://...
set MongoDbSettings__DatabaseName=system_lisoft

# Linux/Mac
export MongoDbSettings__ConnectionString=mongodb+srv://...
export MongoDbSettings__DatabaseName=system_lisoft
```

## 🆘 Problemas Comuns

### "Não foi possível conectar ao MongoDB"
- Verifique se a connection string está correta
- Verifique se seu IP está na whitelist do MongoDB Atlas
- Verifique se o usuário/senha estão corretos

### "npm: não pode ser carregado"
Use o comando alternativo:
```bash
node "C:\Program Files\nodejs\node_modules\npm\bin\npm-cli.js" install
node "C:\Program Files\nodejs\node_modules\npm\bin\npm-cli.js" start
```

### "Erro ao compilar backend"
```bash
cd backend/LiSoft.Api
dotnet clean
dotnet restore
dotnet build
```

## 📚 Documentação

- [Backend README](backend/README.md) - Arquitetura e estrutura do backend
- [MongoDB Configuration](backend/LiSoft.MongoDB/Configuration/README.md) - Detalhes do MongoDB

## 🔑 Obtendo Connection String do MongoDB Atlas

1. Acesse [MongoDB Atlas](https://cloud.mongodb.com)
2. Vá em **Database** → **Connect**
3. Escolha **Connect your application**
4. Copie a connection string
5. Substitua `<password>` pela sua senha
6. Cole no `appsettings.json`

Exemplo:
```
mongodb+srv://usuario:senha@cluster.mongodb.net/
```
