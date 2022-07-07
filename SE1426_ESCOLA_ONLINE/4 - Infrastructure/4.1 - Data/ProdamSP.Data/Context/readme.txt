Pre-requisito de geração de modelo é ter a ferramenta rf instalada. Para essa instalação, favor executar o comando abaixo em Command Prompt:

	 dotnet tool install --global dotnet-ef --version 3.0.0

Para gerar modelo:

1 - Na pasta atual, onde esta a classe de contexto, executar comando abaixo:
dotnet ef dbcontext scaffold "Server=db_desenvolvimento.prodam;Database=SE1426;user id=user_se1426;Password=pwd_se1426;" Microsoft.EntityFrameworkCore.SqlServer --context-dir "..\..\..\4 - Infrastructure\4.1 - Data\ProdamSp.Data\Context" -o "..\..\..\3 - Domain\ProdamSP.Domain\Entities" -f --no-build -c SE1426Context -s "..\..\..\0 - Presentation\SE1426.ProdamSP.WebApi"

2 - Como o namespace é gerado baseado no projeto .NetCode que esta configurado com startup. É necessário :

	2.1 - Substituir namespace das Entities, de SE1426.Internet.ProdamSP.MVC para ProdamSP.Domain.Entities;
	2.2 - Substituir namespace da classe de contexto SE1426Context, de SE1426.Internet.ProdamSP.MVC para ProdamSP.Data;
