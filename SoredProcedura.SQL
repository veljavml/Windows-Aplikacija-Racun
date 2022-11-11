USE [Racun2]
GO
/****** Object:  StoredProcedure [dbo].[InsertUpdate4]    Script Date: 11/11/2022 10:02:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[InsertUpdate4]
(
	@RacunID int,
	@DatumKreiranja datetime,
	@DatumIzmene datetime,
	@UkupnaCena float,
	@NazivStatusa varchar(20),
	@Tabela  TabelaParametar2 READONLY 
)
AS
BEGIN
	
		IF @RacunID != 0
		BEGIN
		Update Racun
		set DatumKreiranja=@DatumKreiranja,
			DatumIzmene=@DatumIzmene,
			UkupnaCena=@UkupnaCena,
			NazivStatusa=@NazivStatusa
			where RacunID=@RacunID

			--select * 
			--into #Stavke
			--from Stavka where RacunID=@RacunID

			merge Stavka as Target
			using @Tabela as Source
			on (Source.StavkaID = Target.StavkaID)  

			When Not MATCHED by Target THEN
			INSERT (RacunID, ProizvodID, Kolicina, Ukupno)
			VALUES (Source.RacunID, Source.ProizvodID, Source.Kolicina, Source.Ukupno)

			When MATCHED THEN UPDATE SET
			Target.RacunID= Source.RacunID,
			Target.ProizvodID=Source.ProizvodID,
			Target.Kolicina = Source.Kolicina,
			Target.Ukupno = Source.Ukupno

			WHEN NOT MATCHED BY Source THEN
			DELETE;
		END
		ELSE IF @RacunID = 0
			BEGIN			
				INSERT INTO Racun (DatumKreiranja,DatumIzmene,UkupnaCena,NazivStatusa)
				Values (@DatumKreiranja,@DatumIzmene,@UkupnaCena,@NazivStatusa)
		
				set	@RacunID = SCOPE_IDENTITY()

				insert into Stavka (RacunID, ProizvodID, Kolicina, Ukupno)
				select @RacunId, ProizvodID, Kolicina, Ukupno
				from @Tabela
			END

END
