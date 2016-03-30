<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/rejestr | /historia">
	<items>
		<xsl:for-each select="*">
			<xsl:element name="item">
				<xsl:for-each select="*">
					<xsl:attribute name="{name()}"><xsl:value-of select="."/></xsl:attribute>
				</xsl:for-each>
			</xsl:element>
		</xsl:for-each>
		</items>
	</xsl:template>
</xsl:stylesheet>
