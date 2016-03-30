<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/pozycja">
	<table class="grid fullWidth">
		<tr>
			<th>Numer dziennikowy</th>
			<td><xsl:value-of select="@numerPozycji"/></td>
		</tr>
			<tr>
			<th>Data wpływu</th>
			<td><xsl:value-of select="dataWplywu"/></td>
		</tr>
			<tr>
			<th>Data pisma</th>
			<td><xsl:value-of select="dataPisma"/></td>
		</tr>
			<tr>
                  <xsl:choose>
                    <xsl:when test="faktura/text()">
                        <th>Numer faktury</th>
                    </xsl:when>
                    <xsl:otherwise>
                    <th>Znak pisma</th>
                    </xsl:otherwise>
                </xsl:choose>
			
			<td><xsl:value-of select="numerPisma"/></td>
		</tr>
        <xsl:if test="faktura/text()">
            <tr>
                <th>Kwota faktury</th>
                <td>
                    <xsl:value-of select="kwota"/>
                    <xsl:text> zł</xsl:text>
                </td>
            </tr>
        </xsl:if>
			<tr>
        <xsl:choose>
          <xsl:when test="faktura/text()">
            <th>Nadawca</th>
          </xsl:when>
          <xsl:otherwise>
            <th>Interesant</th>
          </xsl:otherwise>
        </xsl:choose>
			<td><xsl:value-of select="nadawca"/></td>
		</tr>
			<tr>
			<th>Kategoria dok.</th>
			<td><xsl:value-of select="klasyfikacjaDokumentu/kategoria"/></td>
		</tr>
    <tr>
      <th>Rodzaj dok.</th>
      <td>
        <xsl:value-of select="klasyfikacjaDokumentu/rodzaj"/>
      </td>
    </tr>
    <tr>
      <th>Numer dok.</th>
      <td>
        <xsl:value-of select="klasyfikacjaDokumentu/wartosc"/>
      </td>
    </tr>
			<tr>
			<th>Opis sprawy</th>
			<td><xsl:value-of select="opis"/></td>
		</tr>
	
			<tr>
			<th>Typ koresp.</th>
			<td><xsl:value-of select="typKorespondencji/rodzaj"/><br/><xsl:value-of select="typKorespondencji/wartosc"/></td>
		</tr>
			<tr>
			<th>Uwagi</th>
			<td><xsl:value-of select="uwagi"/></td>
		</tr>
			<tr>
			<th>Wydział/Referat (dekretacja)</th>
			<td><xsl:value-of select="znakReferenta/wydzial"/></td>
		</tr>
			<tr>
			<th>Pismo odebrał</th>
			<td><xsl:value-of select="/pozycja/@pracownik"/></td>
		</tr>
    <tr>
      <th>Dodatkowe niezdigitalizowane materiały</th>
      <td>
        <xsl:choose>
          <xsl:when test="dodatkoweMaterialy/@zawiera">
            <xsl:text>Zawiera: </xsl:text>
            <xsl:value-of select="dodatkoweMaterialy/text()"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>Nie zawiera.</xsl:text>
          </xsl:otherwise>
        </xsl:choose>
      </td>
    </tr>
	</table>
	</xsl:template>
</xsl:stylesheet>
