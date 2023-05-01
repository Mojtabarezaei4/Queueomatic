# Some Notes

### Database
+ SQL as the choice of Database
+ Data models
  - User
    - Name: nvarchar(50)
  - Room
    - Id: int
    - CreatedAt: Date
    - ExpireAt: Date
    - CreatedBy: UserId
    - Name: nvarChar(50)
   - UserRoom
     - UserName: nvarchar(50)
     - RoomId: int
    

### Design patterns and design principles
+ Achiving clean architecture by:
  - Interface segregation principle
  - Single responsibility principle
  - Open-closed principle
  - Common closure principle
+ REPR Design Pattern (Request-Endpoint-Response)
  - We use FastEndpoints to achive this pattern.

