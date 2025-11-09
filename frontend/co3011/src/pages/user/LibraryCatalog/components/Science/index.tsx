import { CustomSlider } from "../../../../../components/slider";
import Widget from "../../../../../components/widget/widget";
import viteLogo from "/vite.svg";

const mockData = [
  {
    title: "Fundamental of Electrical Engineering",
    image: viteLogo,
  },
  {
    title: "Mechanincal Engineering Design",
    image: viteLogo,
  },
  {
    title: "Civil Engineering Materials",
    image: viteLogo,
  },
  {
    title: "Introduction to Chemical Engineering",
    image: viteLogo,
  },
  {
    title: "Biotechnology for Beginners",
    image: viteLogo,
  },
];

const responsive = {
  superLargeDesktop: {
    // the naming can be any, depends on you.
    breakpoint: { max: 4000, min: 3000 },
    items: 5,
  },
  desktop: {
    breakpoint: { max: 3000, min: 1024 },
    items: 3,
  },
  tablet: {
    breakpoint: { max: 1024, min: 464 },
    items: 2,
  },
  mobile: {
    breakpoint: { max: 464, min: 0 },
    items: 1,
  },
};

export function Science() {
  return (
    <>
      <h3
        style={{
          fontWeight: 600,
          textTransform: "capitalize",
          marginBottom: 10,
        }}
      >
        engineering & applied siences
      </h3>

      <CustomSlider
        carouselProps={{
          responsive: responsive,
          children: mockData.map((item) => (
            <Widget marginLeft={30}>
              <p>{item.title}</p>
              <img src={item.image} className="logo" alt="Vite logo" />
            </Widget>
          )),
          infinite: true,
        }}
        style={{
          paddingBlock: 10,
        }}
      />
    </>
  );
}
