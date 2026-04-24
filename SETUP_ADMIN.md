# Configuración del primer usuario Admin

Después de aplicar las migraciones, ejecuta este SQL en tu base de datos MySQL
para crear el usuario administrador inicial o promover uno existente:

## Opción A: Crear admin desde cero
```sql
INSERT INTO Users (Username, Email, Password, Role)
VALUES ('Admin', 'admin@libreria.com', 'admin123', 'Admin');
```

## Opción B: Promover un usuario existente
```sql
UPDATE Users SET Role = 'Admin' WHERE Email = 'tu@email.com';
```

## Aplicar migración de la columna Role
```bash
dotnet ef database update
```

> ⚠️ El campo Role se agrega con valor por defecto 'User' para todos los existentes.
