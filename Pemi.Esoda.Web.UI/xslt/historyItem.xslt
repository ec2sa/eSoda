<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/historia">
		<historia>
			<xsl:for-each select="*">
				<xsl:element name="{name()}">
					<xsl:for-each select="@*">
						<xsl:attribute name="{name()}"><xsl:value-of select="."/></xsl:attribute>
					</xsl:for-each>
					<xsl:for-each select="*">
						<xsl:choose>
							<xsl:when test="name()!='skany'">
								<xsl:element name="{name()}">					
									<xsl:for-each select="@*">
										<xsl:attribute name="{name()}"><xsl:value-of select="."/></xsl:attribute>
									</xsl:for-each>
									<xsl:for-each select="*">
									<xsl:element name="{name()}"><xsl:value-of select="."/></xsl:element>
									</xsl:for-each>
									<xsl:value-of select="./text()"/>
								</xsl:element>
							</xsl:when>
							<xsl:otherwise>
								<xsl:element name="skany">
									<xsl:for-each select="./skan">
										<xsl:value-of select="concat(@nazwa,' ')"/>
									</xsl:for-each>
								</xsl:element>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
				</xsl:element>
			</xsl:for-each>
		</historia>
	</xsl:template>
</xsl:stylesheet>
