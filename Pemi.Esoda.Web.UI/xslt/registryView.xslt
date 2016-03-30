<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
    <xsl:output method="html" version="1.0" encoding="UTF-8" indent="yes" omit-xml-declaration="yes" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"/>
    <xsl:param name="history"/>
    <xsl:variable name="leafCount">
        <xsl:value-of select="count(//pole[not(*)])"/>
    </xsl:variable>
    <xsl:variable name="maxDepth">
        <xsl:for-each select="//pole">
            <xsl:sort select="count(ancestor::pole)" data-type="number"/>
            <xsl:if test="position()=last()">
                <xsl:copy-of select="count(ancestor::pole)+1"/>
            </xsl:if>
        </xsl:for-each>
    </xsl:variable>
    <xsl:variable name="columnCount">
        <xsl:value-of select="count(//pole[(not(*) and count(ancestor::pole[@tworzKolumny='0'])=0) or (@tworzKolumny='0')])"/>
    </xsl:variable>
    <xsl:template match="/">

        <thead>
            <xsl:call-template name="renderLevel">
                <xsl:with-param name="level" select="1"/>
            </xsl:call-template>
        </thead>
        <tbody>
            <xsl:for-each select="/pozycje/pozycja">
                <tr>
                    <xsl:if test="wpis/@data">
                        <td>
                            <xsl:value-of select="wpis/@data"/>
                        </td>
                    </xsl:if>
                    <xsl:if test="wpis/@pracownik">
                        <td>
                            <xsl:value-of select="wpis/@pracownik"/>
                        </td>
                    </xsl:if>
                    <xsl:call-template name="renderColumn">
                        <xsl:with-param name="fieldName">null</xsl:with-param>
                        <xsl:with-param name="rowNumber" select="position()"/>
                    </xsl:call-template>
                    <xsl:if test="$history!='true' and @id">
                        <td>
                            <a href="{concat('../Rejestry/HistoriaRejestru.aspx?id=',@id)}">Historia</a>
                            <br/>
                          <xsl:if test="/pozycje/@isArchive=0">
                                <a href="{concat('../Akcje/EdycjaRejestru.aspx?itemId=',@id,'&amp;regId=',/pozycje/@id)}">Edycja</a>
                          </xsl:if>
                           
                        </td>
                        <xsl:if test="@idDokumentu and (@link=1)">
                            <td>
                                <a href="{concat('../Dokumenty/Dokument.aspx?id=',@idDokumentu)}">
                                    zobacz
                                </a>
                            </td>
                        </xsl:if>
                        <xsl:if test="@idSprawy and (@link=1)">
                            <td>
                                <a href="{concat('../Sprawy/Sprawa.aspx?id=',@idSprawy)}">
                                    zobacz
                                </a>
                            </td>
                        </xsl:if>
                    </xsl:if>
                </tr>
            </xsl:for-each>
        </tbody>

    </xsl:template>
    
    <xsl:template name="renderLevel">
        <xsl:param name="level"/>
        <xsl:if test="$level&lt;=$maxDepth and count(//pole[count(ancestor::pole)=$level - 1 and count(ancestor::pole[@tworzKolumny='0'])=0])&gt;0">
            <tr>
                <xsl:if test="$level='1'  and /pozycje/pozycja/wpis/@data">
                    <th rowspan="{$maxDepth}">Data wpisu</th>
                </xsl:if>
                <xsl:if test="$level='1'  and /pozycje/pozycja/wpis/@pracownik">
                    <th rowspan="{$maxDepth}">Pracownik</th>
                </xsl:if>
                <xsl:for-each select="//pole[count(ancestor::pole)=$level - 1 and count(ancestor::pole[@tworzKolumny='0'])=0]">
                    <xsl:element name="th">
                        <xsl:if test="count(descendant::pole[not(*)])&gt;1">
                            <xsl:attribute name="colspan">
                                <xsl:value-of select="count(descendant::pole[not(*)])"/>
                            </xsl:attribute>
                        </xsl:if>
                        <xsl:if test="($level&lt;$maxDepth and count(descendant::pole)=0) or @tworzKolumny='0'">
                            <xsl:attribute name="rowspan">
                                <xsl:value-of select="$maxDepth - $level + 1"/>
                            </xsl:attribute>
                        </xsl:if>
                        <xsl:value-of select="@etykieta"/>
                    </xsl:element>
                </xsl:for-each>
                <xsl:if test="$history!='true' and /pozycje/pozycja/@id">
                    <th>Operacje</th>
                    <xsl:if test="$level='1'  and (/pozycje/pozycja/@idDokumentu or /pozycje/pozycja/@idSprawy)">
                        <th rowspan="{$maxDepth}">
                            dokument<br/>lub<br/>sprawa
                        </th>
                    </xsl:if>
                </xsl:if>
            </tr>
            <xsl:call-template name="renderLevel">
                <xsl:with-param name="level" select="$level+1"/>
            </xsl:call-template>
        </xsl:if>
    </xsl:template>
    <xsl:template name="renderLeaf">
        <xsl:param name="fieldName"/>
        <xsl:param name="rowNumber"/>
        <td>
            <xsl:value-of select="//pozycja[$rowNumber]/wpis//*[name()=$fieldName]"/>
        </td>
    </xsl:template>
    <xsl:template name="renderNonColumns">
        <xsl:param name="fieldName"/>
        <xsl:param name="rowNumber"/>
        <xsl:variable name="colspan">
            <xsl:value-of select="count(//pole[@nazwa=$fieldName]//pole[not(*)])"/>
        </xsl:variable>
        <xsl:element name="td">
            <xsl:if test="$colspan&gt;1">
                <xsl:attribute name="colspan">
                    <xsl:value-of select="$colspan"/>
                </xsl:attribute>
            </xsl:if>
            <xsl:for-each select="/pozycje/pozycja[$rowNumber]/wpis//*[name()=$fieldName]//*[not(*)]">
                <xsl:variable name="fn" select="name()"/>
                <xsl:if test="string-length(/pozycje/pola//pole[@nazwa=$fn]/@prefiks)&gt;0">
                    <strong>
                        <xsl:value-of select="/pozycje/pola//pole[@nazwa=$fn]/@prefiks"/>&#160;
                    </strong>
                </xsl:if>
                <xsl:value-of select="text()"/>
                <xsl:choose>
                    <xsl:when test="/pozycje/pola//pole[@nazwa=$fn][@zawijaj='1']">
                        <br/>
                    </xsl:when>
                    <xsl:otherwise>
                        &#160;
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:for-each>
        </xsl:element>
    </xsl:template>
    
    <xsl:template name="renderColumn">
        <xsl:param name="fieldName"/>
        <xsl:param name="rowNumber"/>
        <xsl:choose>
            <xsl:when test="$fieldName='null'">
                <xsl:for-each select="/pozycje/pola/pole">
                    <xsl:choose>
                        <xsl:when test="not(*)">
                            <xsl:call-template name="renderLeaf">
                                <xsl:with-param name="fieldName" select="@nazwa"/>
                                <xsl:with-param name="rowNumber" select="$rowNumber"/>
                            </xsl:call-template>
                        </xsl:when>
                        <xsl:when test=" @tworzKolumny='0'">
                            <xsl:call-template name="renderNonColumns">
                                <xsl:with-param name="fieldName" select="@nazwa"/>
                                <xsl:with-param name="rowNumber" select="$rowNumber"/>
                            </xsl:call-template>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:call-template name="renderColumn">
                                <xsl:with-param name="fieldName" select="@nazwa"/>
                                <xsl:with-param name="rowNumber" select="$rowNumber"/>
                            </xsl:call-template>
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:for-each>
            </xsl:when>
            <xsl:otherwise>
                <xsl:for-each select="/pozycje/pola//pole[@nazwa=$fieldName]/pole">
                    <xsl:choose>
                        <xsl:when test="not(*)">
                            <xsl:call-template name="renderLeaf">
                                <xsl:with-param name="fieldName" select="@nazwa"/>
                                <xsl:with-param name="rowNumber" select="$rowNumber"/>
                            </xsl:call-template>
                        </xsl:when>
                        <xsl:when test=" @tworzKolumny='0'">
                            <xsl:call-template name="renderNonColumns">
                                <xsl:with-param name="fieldName" select="@nazwa"/>
                                <xsl:with-param name="rowNumber" select="$rowNumber"/>
                            </xsl:call-template>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:call-template name="renderColumn">
                                <xsl:with-param name="fieldName" select="@nazwa"/>
                                <xsl:with-param name="rowNumber" select="$rowNumber"/>
                            </xsl:call-template>
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:for-each>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
</xsl:stylesheet>
