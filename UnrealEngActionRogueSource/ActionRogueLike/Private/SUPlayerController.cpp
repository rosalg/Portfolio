// Fill out your copyright notice in the Description page of Project Settings.


#include "SUPlayerController.h"
#include "Blueprint/UserWidget.h"


void ASUPlayerController::TogglePauseMenu()
{

	if (PauseMenuInstance && PauseMenuInstance->IsInViewport()) {
		PauseMenuInstance->RemoveFromParent();
		PauseMenuInstance = nullptr;

		bShowMouseCursor = false;
		SetInputMode(FInputModeGameOnly());
		return;
	}

	PauseMenuInstance = CreateWidget<UUserWidget>(this, PauseMenuClass);
	if (PauseMenuInstance) {
		PauseMenuInstance->AddToViewport(100);

		bShowMouseCursor = true;
		SetInputMode(FInputModeUIOnly());


	}
}

void ASUPlayerController::SetupInputComponent()
{
	Super::SetupInputComponent();

	InputComponent->BindAction("PauseMenu", IE_Pressed, this, &ASUPlayerController::TogglePauseMenu);
}

void ASUPlayerController::SetPawn(APawn* InPawn) {
	Super::SetPawn(InPawn);

	OnPawnChanged.Broadcast(InPawn);
}
