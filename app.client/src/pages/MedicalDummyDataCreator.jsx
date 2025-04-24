import { Button, Typography, DatePicker, Radio, Input, Col, Row, Checkbox } from 'antd';
const { Text } = Typography;
const MedicalDummyDataCreator = () => {
    const onChange = (date, dateString) => {
        console.log(date, dateString);
    };

    const optionsTableCheckBox = [
        { label: 'ClientAddInfomation', value: 'ClientAddInfomation' },
        { label: 'ClientAddInfomation2', value: 'ClientAddInfomation2' },
        { label: 'ClientInfomation', value: 'ClientInfomation' },
        { label: 'MedicalCheckData', value: 'MedicalCheckData' },
        { label: 'MedicalCheckItem', value: 'MedicalCheckItem' },
        { label: 'MedicalCheckState', value: 'MedicalCheckState' },
        { label: 'NextGuideData', value: 'NextGuideData' },
    ];
    return (
        <div style={{ maxWidth: 800, backgroundColor: '#d7e4f2', margin: '10px auto', padding: '10px', borderRadius: '8px' }}>
            <Row>
                <Col span={12}>
                    <Row >
                        <Text>案件名選択</Text>
                        <Input style={{ margin: 4 }} />
                    </Row>
                    <Text>パターン登録名</Text>
                    <Row >
                        <Col span={15}>
                            <Input style={{ margin: 4 }} />
                        </Col>
                        <Col span={1}>
                        </Col>
                        <Col span={4}>
                            <Button style={{ margin: 4 }} type="primary">登録</Button>
                        </Col>
                        <Col span={4}>
                            <Button style={{ margin: 4 }} type="primary">過去</Button>
                        </Col>
                    </Row>
                    <div style={{ maxWidth: 800, backgroundColor: '#fff', margin: '10px auto', padding: '20px', borderRadius: '8px' }}>
                        <Row>
                            <Text style={{ fontSize: 12 }}>MedicalCheckDate</Text>
                        </Row>

                        <Row>
                            <Col span={12}>
                                <DatePicker onChange={onChange} />
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
                            <Col span={6}>
                                <Input />
                            </Col>
                            <Col span={1} style={{ margin: '0 auto' }}>
                                <Text >~</Text>
                            </Col>
                            <Col span={6}>
                                <Input />
                            </Col>
                            <Col span={2}>

                            </Col>
                            <Col span={4}>
                                <Button >+</Button>
                            </Col>
                            <Col span={4}>
                                <Button >-</Button>
                            </Col>
                        </Row>
                        <Row>
                            <Text style={{ fontSize: 12 }}>Division</Text>
                        </Row>
                        <Input />

                    </div>
                    <div style={{ maxWidth: 800, backgroundColor: '#fff', margin: '10px auto', padding: '20px', borderRadius: '8px' }}>
                        <Row>
                            <Text style={{ fontSize: 12 }}>※削除・日付更新条件対象外</Text>
                        </Row>
                        <Row>
                            <Text style={{ fontSize: 12 }}>性別</Text>
                        </Row>
                        <Row>
                            <Radio.Group
                                name="radiogroup"
                                defaultValue={1}
                                options={[
                                    { value: 1, label: '男' },
                                    { value: 2, label: '女' },
                                ]}
                            />
                        </Row>

                        <Row>
                            <Text style={{ fontSize: 12 }}>生年月日</Text>
                        </Row>

                        <Row>
                            <Col span={12}>
                                <DatePicker onChange={onChange} />
                            </Col>

                        </Row>
                        <Row>
                            <Text style={{ fontSize: 12 }}>コースID</Text>
                        </Row>
                        <Input />

                    </div>
                    <div style={{ maxWidth: 800, backgroundColor: '#fff', margin: '10px auto', padding: '20px', borderRadius: '8px' }}>
                        <Checkbox.Group options={optionsTableCheckBox} />
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
        </div>
    )
}
export default MedicalDummyDataCreator;