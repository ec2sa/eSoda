<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" indent="yes" encoding="utf-8"/>


	<!-- Replace root node name Menus with MenuItems
       and call MenuListing for its children-->
	<xsl:template match="/JRWA">
		<JRWA>
			<xsl:call-template name="JRWAItems" />
		</JRWA>
	</xsl:template>

	<!-- Allow for recursive child nodeprocessing -->
	<xsl:template name="JRWAItems">
		<xsl:apply-templates select="JRWAItem" />
	</xsl:template>

	<xsl:template match="JRWAItem">
		<JRWAItem>
			<!-- id, idRodzica, symbol, nazwa, kategoriaAKM, kategoriaAIK, uwagi, aktywny -->
			<xsl:attribute name="Text">
				<xsl:value-of select="symbol"/>
			</xsl:attribute>

			<xsl:attribute name="Value">
				<xsl:value-of select="nazwa"/>
			</xsl:attribute>

			<xsl:attribute name="ToolTip">
				<xsl:value-of select="kategoriaAKM"/>
			</xsl:attribute>

			<xsl:attribute name="NavigateURL">
				<xsl:value-of select="kategoriaAIK"/>
			</xsl:attribute>

			<xsl:attribute name="Target">
				<xsl:value-of select="uwagi"/>
			</xsl:attribute>

			<!-- Recursively call MenuListing forchild menu nodes -->
			<xsl:if test="count(JRWAItem) >0">
				<xsl:call-template name="JRWAItems" />
			</xsl:if>

		</JRWAItem>
	</xsl:template>
</xsl:stylesheet>