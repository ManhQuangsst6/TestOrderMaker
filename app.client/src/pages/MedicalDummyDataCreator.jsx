import { Button, Typography, Radio, Input, Col, Row, Checkbox } from 'antd';
import PaddedNumberInput from '../bases/InputNumberBase';
import DatePickerBase from '../bases/DatePickerBase';
import CheckboxGroupBase from '../bases/CheckboxGroupBase';
import React, { useState, useEffect } from 'react';
import dayjs from 'dayjs';
import { PostMedical } from '../common/api-services/api/medical';
import { useParams } from 'react-router-dom';
const { Text } = Typography;

const MedicalDummyDataCreator = () => {
    const { projectNameParam } = useParams();
    const [info, setInfo] = useState({
        NameProject: "",
        MedicalCheckDate: dayjs(),
        ClientIDStart: "00001",
        ClientIDEnd: "00001",
        Division: "",
        Sex: 1,
        BirthDay: dayjs(),
        CourseId: "",
        TableInsert: []
    })
    useEffect(() => {
        setInfo(prevState => ({
            ...prevState,
            NameProject: projectNameParam
        }));
    }, [projectNameParam]);
    const handleChange = (field, value) => {
        setInfo((prev) => ({ ...prev, [field]: value }));
    };
    const optionsTableCheckBox = [
        { label: 'ClientAddInfomation', value: 'ClientAddInformation' },
        { label: 'ClientAddInfomation2', value: 'ClientAddInformation2' },
        { label: 'ClientInfomation', value: 'ClientInformation' },
        { label: 'MedicalCheckData', value: 'MedicalCheckData' },
        { label: 'MedicalCheckItem', value: 'MedicalCheckItem' },
        { label: 'MedicalCheckState', value: 'MedicalCheckState' },
        { label: 'NextGuideData', value: 'NextGuideData' },
    ];
    const CreateInfo = () => {
        console.log(info);
        PostMedical(info).then((res) => {
            console.log(res.data);
        }).catch((err) => {
            console.error(err);
        });
    }

    return (
        <div style={{ maxWidth: 800, backgroundColor: '#d7e4f2', margin: '5px auto', padding: '10px 20px', borderRadius: '8px' }}>
            <Row>
                <Col span={12}>
                    <Row >
                        <Text>案件名選択</Text>
                        <Input style={{ margin: 2 }} />
                    </Row>
                    <Text>パターン登録名</Text>
                    <Row >
                        <Col span={15}>
                            <Input style={{ margin: 2 }} />
                        </Col>
                        <Col span={1}>
                        </Col>
                        <Col span={4}>
                            <Button size='small' style={{ margin: 2 }} type="primary">登録</Button>
                        </Col>
                        <Col span={4}>
                            <Button size='small' style={{ margin: 2 }} type="primary">過去</Button>
                        </Col>
                    </Row>
                    <div style={{ maxWidth: 800, backgroundColor: '#fff', margin: '0px auto', padding: '20px', borderRadius: '8px' }}>
                        <Row>
                            <Text style={{ fontSize: 12 }}>MedicalCheckDate</Text>
                        </Row>

                        <Row>
                            <Col span={12}>
                                <DatePickerBase
                                    value={dayjs(info.MedicalCheckDate)}
                                    onChange={(date) => handleChange('MedicalCheckDate', date?.format('YYYY-MM-DD'))}
                                    placeholder=""
                                />
                            </Col>
                            <Col span={2}>

                            </Col>
                            <Col span={5}>
                                <Button >過去</Button>
                            </Col>
                            <Col span={5}>
                                <Button >現在</Button>
                            </Col>
                        </Row>

                        <Row>
                            <Text style={{ fontSize: 12 }}>ClientID</Text>
                        </Row>

                        <Row>
                            <Col span={11}>
                                <PaddedNumberInput
                                    value={info.ClientIDStart}
                                    onChange={(e) => handleChange('ClientIDStart', e)}
                                    length={5}
                                    min={0}
                                    max={9999}
                                />
                            </Col>
                            <Col span={2} style={{ margin: '0 auto' }}>
                                <Text >~</Text>
                            </Col>
                            <Col span={11}>
                                <PaddedNumberInput
                                    value={info.ClientIDEnd}
                                    onChange={(e) => handleChange('ClientIDEnd', e)}
                                    length={4}
                                    min={0}
                                    max={9999}
                                />
                            </Col>
                        </Row>
                        <Row>
                            <Text style={{ fontSize: 12 }}>Division</Text>
                        </Row>
                        <Input onChange={(e) => handleChange('Division', e.target.value)} size='small' />

                    </div>
                    <div style={{ maxWidth: 800, backgroundColor: '#fff', margin: '5px auto', padding: '20px', borderRadius: '8px' }}>
                        <Row>
                            <Text style={{ fontSize: 12 }}>※削除・日付更新条件対象外</Text>
                        </Row>
                        <Row>
                            <Col span={12}><Text style={{ fontSize: 12 }}>性別</Text></Col>
                            <Col span={12}>  <Text style={{ fontSize: 12 }}>生年月日</Text></Col>
                        </Row>
                        <Row>
                            <Col span={12}>
                                <Radio.Group
                                    name="radiogroup"
                                    value={info.Sex}
                                    onChange={(e) => handleChange('Sex', e.target.value)}
                                    options={[
                                        { value: 1, label: '男' },
                                        { value: 2, label: '女' },
                                    ]}
                                />
                            </Col>
                            <Col span={12}>
                                <DatePickerBase
                                    value={dayjs(info.BirthDay)}
                                    onChange={(date) => handleChange('BirthDay', date?.format('YYYY-MM-DD'))}
                                    placeholder=""
                                />
                            </Col>
                        </Row>


                        <Row>
                            <Text style={{ fontSize: 12 }}>コースID</Text>
                        </Row>
                        <Input value={info.CourseId}
                            onChange={(e) => handleChange('CourseId', e.target.value)} size='small' />

                    </div>
                    <div style={{ maxWidth: 800, backgroundColor: '#fff', margin: '5px auto', padding: '10px', borderRadius: '8px' }}>
                        <CheckboxGroupBase
                            options={optionsTableCheckBox}
                            value={info.TableInsert}
                            onChange={(checkedValues) => handleChange('TableInsert', checkedValues)}
                            style={{
                                display: 'flex',
                                flexDirection: 'column',
                            }}

                        />

                    </div>
                </Col>
                <Col span={12} style={{ paddingLeft: 10 }}>
                    <Row>
                        <Col span={10}>
                            <Button  >すべてチェック解除</Button>
                        </Col>
                        <Col span={10}>
                            <Button  >すべて閉じる</Button>
                        </Col>
                        <div style={{ maxWidth: 800, backgroundColor: '#fff', margin: '10px auto', padding: '20px', borderRadius: '8px' }}>
                            <Checkbox.Group options={optionsTableCheckBox} />
                        </div>

                    </Row>
                </Col>
            </Row>
            <Row style={{ marginTop: 10 }}>
                <Col span={12}>
                    <Button size='large' type="primary" >一致条件レコードを削除</Button>
                </Col>
                <Col span={12}>
                    <Button size='large' type="primary" onClick={CreateInfo}>レコード新規作成</Button>
                </Col>

            </Row>
        </div>
    )
}
export default MedicalDummyDataCreator;