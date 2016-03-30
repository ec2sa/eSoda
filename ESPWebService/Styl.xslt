<?xml version="1.0" encoding="UTF-8"?><xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:wnio="http://crd.gov.pl/wzor/2008/05/09/3/" xmlns:inst="http://crd.gov.pl/xml/schematy/instytucja/2008/05/09/" xmlns:str="http://crd.gov.pl/xml/schematy/struktura/2008/05/09/" xmlns:oso="http://crd.gov.pl/xml/schematy/osoba/2008/05/09/" xmlns:meta="http://crd.gov.pl/xml/schematy/meta/2008/05/09/" xmlns:adr="http://crd.gov.pl/xml/schematy/adres/2008/05/09/" xmlns:xades="http://uri.etsi.org/01903/v1.3.2#" xmlns:ds="http://www.w3.org/2000/09/xmldsig#">
	<xsl:output method="html" encoding="utf-8" indent="yes"/>
	<xsl:template match="/wnio:Dokument">
		<html>
			<head>
				<style type="text/css">.tekst{ font-family: Arial; font-size: 10pt; color: #000000; text-align: center;}</style>
				<style type="text/css">.naglowek{ font-family: Arial; font-size: 12pt; font-weight: bold; color: #000000; }</style>
				<style type="text/css">.strona { width: 16cm; margin: 0 auto;text-align: justify;}</style>
				<style type="text/css">.element{ font-family: Arial; font-size: 10pt; color: #800000; }</style>
			
				<style type="text/css">.naglowek{ font-family: Arial; font-size: 12pt; font-weight: bold; color: #000000; }</style>
		
			</head>
			<body class="tekst">
				<div class="strona">
					<xsl:call-template name="Gora"/>
					<xsl:call-template name="MojUrzad"/>
					<xsl:call-template name="Tytul">
						<xsl:with-param name="tresc" select="wnio:TrescDokumentu/wnio:Wartosc/wnio:RodzajWniosku/wnio:TytulDokumentu"/>
					</xsl:call-template>
					<br/>
					<br/>
					<xsl:for-each select="wnio:TrescDokumentu/wnio:Wartosc/wnio:TrescWniosku">
						<span class="naglowek">
							<xsl:value-of select="wnio:NaglowekTresci"/>
						</span>
						<br/>
						<xsl:value-of select="wnio:ZawartoscTresci"/>
						<br/>
					</xsl:for-each>
					<br/>
					<br/>
					<xsl:for-each select="wnio:TrescDokumentu/wnio:Wartosc/wnio:Zalaczniki">
						<xsl:if test="count(.) &gt; 0">
							<p>
								<span class="naglowek">ZAŁĄCZNIKI</span>
							</p>
						</xsl:if>
					
							<xsl:for-each select="str:Zalacznik">
								<xsl:choose>
									<xsl:when test="@kodowanie='URI'">
									<A>
									<xsl:attribute name="HREF"><xsl:value-of select="str:DaneZalacznika"/></xsl:attribute>
									<xsl:attribute name="TARGET">_blank</xsl:attribute>
									<B>
										<xsl:value-of select="@nazwaPliku"/>
									</B>
								</A>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="@nazwaPliku"/>
									</xsl:otherwise>
								</xsl:choose><br/>
							</xsl:for-each>
						
					</xsl:for-each>
					<br/><br/>
						<span class="naglowek">
					<xsl:text>Dane dotyczące podpisu: </xsl:text>
				</span>
				<br/>
				<xsl:choose>
					<xsl:when test="//wnio:Dokument/ds:Signature!=''">
						<xsl:text>Dokument został podpisany - aby go zweryfikować należy użyć oprogramowania do weryfikacji podpisu</xsl:text>
						<br/>
						<span class="element">
							<xsl:text>Lista podpisanych elementów (referencji):</xsl:text>
						</span>
						<br/>
						<xsl:for-each select="//wnio:Dokument/ds:Signature/ds:SignedInfo/ds:Reference">
							<span class="element">
								<xsl:text>refenrencja </xsl:text>
								<xsl:value-of select="@Id"/>
								<xsl:text> : </xsl:text></span>
								<xsl:value-of select="@URI"/>
								<br/>
							
						</xsl:for-each>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>Dokument nie zawiera podpisu </xsl:text>
					</xsl:otherwise>
				</xsl:choose>
					
					
				</div>
			</body>
		</html>
	</xsl:template>
	<xsl:template name="MojUrzad">
		<xsl:for-each select="//wnio:DaneDokumentu/str:Adresaci/meta:Podmiot/inst:Instytucja">
			<p style="text-align:right">
				<xsl:value-of select="inst:NazwaInstytucji"/>
			</p>
			<xsl:for-each select="adr:Adres">
				<p style="text-align:right">
				w
				<xsl:value-of select="adr:Miejscowosc"/>
				</p>
				<p style="text-align:right">
				ul.
				<xsl:value-of select="adr:Ulica"/>
					<xsl:value-of select="' '"/>
				nr
				<xsl:value-of select="adr:Budynek"/>
					<xsl:value-of select="' '"/>
					<xsl:value-of select="adr:Lokal"/>
				</p>
				<p style="text-align:right">
					<xsl:value-of select="adr:KodPocztowy"/>
					<xsl:value-of select="' '"/>
					<xsl:value-of select="adr:Poczta"/>
				</p>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="LewyGorny">
		<xsl:for-each select="wnio:DaneDokumentu/str:Nadawcy/meta:Podmiot">
			<xsl:for-each select="oso:Osoba">
				<p>
					<xsl:value-of select="oso:Imie"/>
				</p>
				<p>
					<xsl:value-of select="oso:Nazwisko[@rodzajCzlonu='pierwszy']"/>
				-
				<xsl:value-of select="oso:Nazwisko[@rodzajCzlonu='drugi']"/>
				</p>
			</xsl:for-each>
			<xsl:for-each select="inst:Instytucja">
				<p>
					<xsl:value-of select="inst:NazwaInstytucji"/>
				</p>
				<xsl:for-each select="inst:IdInstytucji/inst:NIP">
					<xsl:call-template name="Para">
						<xsl:with-param name="etykieta">NIP</xsl:with-param>
					</xsl:call-template>
				</xsl:for-each>
			</xsl:for-each>
			<xsl:for-each select="inst:Instytucja/adr:Adres">
				<xsl:call-template name="Adres"/>
			</xsl:for-each>
			<xsl:for-each select="oso:Osoba/adr:Adres">
				<xsl:call-template name="Adres"/>
			</xsl:for-each>
			<xsl:for-each select="oso:Osoba">
				<xsl:for-each select="oso:IdOsoby/oso:NIP">
					<xsl:call-template name="Para">
						<xsl:with-param name="etykieta">NIP</xsl:with-param>
					</xsl:call-template>
				</xsl:for-each>
				<xsl:for-each select="oso:IdOsoby/oso:PESEL">
					<xsl:call-template name="Para">
						<xsl:with-param name="etykieta">
						PESEL
					</xsl:with-param>
					</xsl:call-template>
				</xsl:for-each>
			</xsl:for-each>
			<xsl:for-each select="meta:Instytucja">
				<xsl:for-each select="inst:IdInstytucji/inst:KRS">
					<xsl:call-template name="Para">
						<xsl:with-param name="etykieta">KRS</xsl:with-param>
					</xsl:call-template>
				</xsl:for-each>
				<xsl:for-each select="inst:IdInstytucji/inst:REGON">
					<xsl:call-template name="Para">
						<xsl:with-param name="etykieta">
						REGON
					</xsl:with-param>
					</xsl:call-template>
				</xsl:for-each>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="Para">
		<xsl:param name="etykieta"/>
		<xsl:if test="string-length(.) &gt; 0">
			<p>
				<xsl:value-of select="$etykieta"/>
				:
				<xsl:value-of select="."/>
			</p>
		</xsl:if>
	</xsl:template>
	<xsl:template name="PrawyGorny">
		<xsl:value-of select="wnio:TrescDokumentu/wnio:Wartosc/wnio:RodzajWniosku/wnio:MiejscowoscDokumentu"/>,
		<xsl:value-of select="wnio:OpisDokumentu/meta:Data/meta:Czas"/>
	</xsl:template>
	<xsl:template name="Gora">
		<div style="width:99%;padding: 1px 1px;">
			<div style="width:50%;float:left;text-align:left;vertical-align:top">
				<xsl:call-template name="LewyGorny"/>
			</div>
			<div style="width:50%;float:right;text-align:right;vertical-align:top">
				<xsl:call-template name="PrawyGorny"/>
			</div>
			<div style="clear:both"/>
		</div>
		<br/>
		<br/>
		<br/>
		<br/>
	</xsl:template>
	<xsl:template name="Tytul">
		<xsl:param name="tresc"/>
		<br/>
		<br/>
		<br/>
		<br/>
		<br/>
		<p style="text-align:center">
			<b>
				<span style="font-size:12.0pt">
					<xsl:value-of select="$tresc"/>
				</span>
			</b>
		</p>
		<br/>
	</xsl:template>
	<xsl:template name="Adres">
		<p>
			<xsl:value-of select="adr:Ulica/@rodzajUlicy"/>
			<xsl:value-of select="' '"/>
			<xsl:value-of select="adr:Ulica"/>
			<xsl:value-of select="' '"/>
			<xsl:value-of select="adr:Budynek"/>/<xsl:value-of select="adr:Lokal"/>
		</p>
		<p>
			<xsl:value-of select="adr:Miejscowosc"/>
		</p>
		<p>
			<xsl:value-of select="adr:KodPocztowy"/>
			<xsl:value-of select="' '"/>
			<xsl:value-of select="adr:Poczta"/>
		</p>
	</xsl:template>
	
</xsl:stylesheet>