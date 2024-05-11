// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "ProceduralMeshComponent.h"
#include "WorldGenerator.generated.h"

UCLASS()
class SEAGULLS_API AWorldGenerator : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AWorldGenerator();

	UPROPERTY(EditAnywhere, BlueprintReadOnly)
		int XVertexCount = 50;

	UPROPERTY(EditAnywhere, BlueprintReadOnly)
		int YVertexCount = 50;

	UPROPERTY(EditAnywhere, BlueprintReadOnly)
		float CellSize = 1000;
	
	UPROPERTY(EditAnywhere, BlueprintReadOnly)
		int NumOfSectionsX = 2;
	
	UPROPERTY(EditAnywhere, BlueprintReadOnly)
		int NumOfSectionsY = 2;
	
	UPROPERTY(EditAnywhere, BlueprintReadWrite)
		int MeshSectionIndex = 0;
	
	UPROPERTY(BlueprintReadOnly)
		UProceduralMeshComponent* TerrainMesh;

	UPROPERTY(EditAnywhere, BlueprintReadWrite)
		UMaterialInterface* TerrainMaterial = nullptr;

	
	
protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	UFUNCTION(BlueprintCallable)
		void GenerateTerrain(const int SectionIndexX, const int SectionIndexY);

	float GetHeight(const FVector2D Location);
	float PerlinNoiseExtended(const FVector2D Location, const float Scale, const float Amplitude, const FVector2D offset);
};
