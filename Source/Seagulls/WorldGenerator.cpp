// Fill out your copyright notice in the Description page of Project Settings.


#include "WorldGenerator.h"
#include "KismetProceduralMeshLibrary.h"

#include "IPropertyTable.h"
#include "MeshBuild.h"

// Sets default values
AWorldGenerator::AWorldGenerator()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

	TerrainMesh = CreateDefaultSubobject<UProceduralMeshComponent>(TEXT("TerrainMesh"));
	TerrainMesh -> SetupAttachment(GetRootComponent());

}

// Called when the game starts or when spawned
void AWorldGenerator::BeginPlay()
{
	Super::BeginPlay();
	
}

// Called every frame
void AWorldGenerator::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

void AWorldGenerator::GenerateTerrain(const int SectionIndexX, const int SectionIndexY)
{
	FVector Offset = FVector(SectionIndexX*(XVertexCount-1), SectionIndexY*(YVertexCount-1), 0.f)*CellSize;

	TArray<FVector> Vertices;
	FVector Vertex;

	TArray<FVector2d> UVs;
	FVector2D UV;

	TArray<int32> Triangles;
	TArray<FVector> Normals;
	TArray<FProcMeshTangent> Tangents;

	//Vertices and UVs
	for (int32 iVY = -1; iVY <= YVertexCount; iVY++)
	{
		for (int32 iVX = -1; iVX <= XVertexCount; iVX++)
		{
			//Vertex Calculation
			Vertex.X = iVX * CellSize + Offset.X;
			Vertex.Y = iVY * CellSize + Offset.Y;
			Vertex.Z = GetHeight(FVector2d(Vertex.X, Vertex.Y));
			Vertices.Add(Vertex);

			//UV
			UV.X = (iVX + (XVertexCount-1) * SectionIndexX) * CellSize / 100;
			UV.Y = (iVY + (YVertexCount-1) * SectionIndexY) * CellSize / 100;
			UVs.Add(UV);
		}
	}

	//Triangles
	for (int32 iTY = 0; iTY <= YVertexCount; iTY++)
	{
		for (int32 iTX = 0; iTX <= XVertexCount; iTX++)
		{
			//Calculation
			Triangles.Add(iTX + iTY * (XVertexCount +2));
			Triangles.Add(iTX + (iTY + 1) * (XVertexCount +2));
			Triangles.Add(iTX + iTY * (XVertexCount +2) +1);
			
			Triangles.Add(iTX + (iTY + 1) * (XVertexCount +2));
			Triangles.Add(iTX + (iTY + 1) * (XVertexCount +2) +1);
			Triangles.Add(iTX + iTY * (XVertexCount +2) +1);
		}
	}

	// Calculating subset mesh to prevent seams
	TArray<FVector> SubVertices;
	TArray<FVector2d> SubUVs;
	TArray<int32> SubTriangles;
	TArray<FVector> SubNormals;
	TArray<FProcMeshTangent> SubTangents;

	int VertexIndex = 0;
	
	// Calculating Normals
	UKismetProceduralMeshLibrary::CalculateTangentsForMesh(Vertices, Triangles, UVs, Normals, Tangents);

	// Subset vertices and UVs
	for (int32 iVY = -1; iVY <= YVertexCount; iVY++)
	{
		for (int32 iVX = -1; iVX <= XVertexCount; iVX++)
		{
			if (iVY > -1 && iVY < YVertexCount && iVX > -1 && iVX < XVertexCount)
			{
				SubVertices.Add(Vertices[VertexIndex]);
				SubUVs.Add(UVs[VertexIndex]);
				SubNormals.Add(Normals[VertexIndex]);
				SubTangents.Add(Tangents[VertexIndex]);
			}
			VertexIndex++;
		}
	}
	
	// Subset triangles
	for (int32 iTY = 0; iTY <= YVertexCount - 2; iTY++)
	{
		for (int32 iTX = 0; iTX <= XVertexCount - 2; iTX++)
		{
			SubTriangles.Add(iTX + iTY * XVertexCount);
			SubTriangles.Add(iTX + iTY * XVertexCount + XVertexCount);
			SubTriangles.Add(iTX + iTY * XVertexCount + 1);
			
			SubTriangles.Add(iTX + iTY * XVertexCount + XVertexCount);
			SubTriangles.Add(iTX + iTY * XVertexCount + XVertexCount +1);
			SubTriangles.Add(iTX + iTY * XVertexCount + 1);
		}
	}
	
	//Create mesh section
	TerrainMesh -> CreateMeshSection(MeshSectionIndex, SubVertices, SubTriangles, SubNormals, SubUVs, TArray<FColor>(), SubTangents, true);
	if (TerrainMesh)
	{
		TerrainMesh -> SetMaterial(MeshSectionIndex, TerrainMaterial);
	}
	MeshSectionIndex++;
}

float AWorldGenerator::GetHeight(FVector2D Location)
{
	return PerlinNoiseExtended(Location, 0.00001f, 20000, FVector2D(0.1f))
	+ PerlinNoiseExtended(Location, 0.0001f, 10000, FVector2D(0.2f))
	+ PerlinNoiseExtended(Location, 0.001f, 500, FVector2D(0.3f))
	+ PerlinNoiseExtended(Location, 0.01f, 100, FVector2D(0.4f));
}

float AWorldGenerator::PerlinNoiseExtended(const FVector2D Location, const float Scale, const float Amplitude, const FVector2D Offset)
{
	return FMath::PerlinNoise2D(Location * Scale + FVector2D(0.1f, 0.1f) + Offset) * Amplitude;
}

